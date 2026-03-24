using ReactMaterialUIShowcaseApi.Entities;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Dtos;
using System;
using Microsoft.EntityFrameworkCore;
using ReactMaterialUIShowcaseApi.Services.Query;
using AutoMapper;

namespace ReactMaterialUIShowcaseApi.Repositories
{
    public class SiteProfileRepository : GenericRepository<SiteProfile>, ISiteProfileRepository
    {
        private readonly IMapper _mapper;

        public SiteProfileRepository(ApplicationDBContext context, IMapper mapper) : base(context) {
            _mapper = mapper;
        }

        public async Task<SiteProfile?> GetSiteProfileWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public new async virtual Task<(IEnumerable<SiteProfile> Items, int Total)> QueryAsync(ParsedListQuery parsed)
        {
            return await _dbSet.Include(t => t.Tags).ApplyListQueryAsync(parsed);
        }

        private async void DeleteTags(SiteProfile entity)
        {
            var existing = await _dbSet.Include(t => t.Tags).FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (existing != null)
            {
                existing.Tags.Clear();
                await _context.SaveChangesAsync();
            }
        }

        private void RemoveUnwantedTags(SiteProfile entity, SiteProfileDto dto)
        {
            var existingSiteProfile = entity;
            if (existingSiteProfile is null) return;

            // 1. Clear out tags that are no longer in the DTO
            var tagsToRemove = existingSiteProfile.Tags
                .Where(oldTag => !dto.Tags.Contains(oldTag.Tag))
                .ToList();

            foreach (var tag in tagsToRemove)
            {
                _context.Remove(tag); // This tells EF to generate a DELETE statement
            }

            // 2. Add tags that are in the DTO but not yet in the Entity
            foreach (var newTagName in dto.Tags)
            {
                if (!existingSiteProfile.Tags.Any(t => t.Tag == newTagName))
                {
                    existingSiteProfile.Tags.Add(new SiteProfileTag { Tag = newTagName });
                }
            }
        }

        public async void Update(SiteProfile entity, SiteProfileDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                RemoveUnwantedTags(entity, dto);
                _mapper.Map(dto, entity);

                _dbSet.Update(entity);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}