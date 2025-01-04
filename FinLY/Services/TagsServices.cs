using FinLY.Components.Pages;
using FinLY.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinLY.Services
{
    public class TagsServices : ITagsServices
    {
        private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseTags.json");

        public async Task AddCustomTagAsync(Models.Tags tag)
        {
          
            try
            {
                var tags = await LoadTagsAsync();

                if (!tag.IsDefault)
                {
                    tag.TagId = Guid.NewGuid();
                    tags.Add(tag);  // Add custom user tag
                    await SaveTagsAsync(tags);
                }
                else
                {
                    Console.WriteLine("Default tags should not be added manually.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Adding Tags: {ex.Message}");
            }
        }

        private async Task SaveTagsAsync(List<Models.Tags> tags)
        {
            try
            {
                var json = JsonSerializer.Serialize(tags, new JsonSerializerOptions { WriteIndented = true }); 
                await File.WriteAllTextAsync(FinLyFilePath, json);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error Saving Tags: {ex.Message}");
                throw;
            }
        }

        private async Task<List<Models.Tags>> LoadTagsAsync()
        {
            try
            {
                if(!File.Exists(FinLyFilePath))
                {
                    return new List<Models.Tags>();
                }

                var json = await File.ReadAllTextAsync(FinLyFilePath);
                return JsonSerializer.Deserialize<List<Models.Tags>>(json) ?? new List<Models.Tags>();
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error Loading Tags: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Models.Tags>> GetTagsByUserIdAsync(Guid UserId)
        {
            try
            {
                //var tags = await LoadTagsAsync();

                //return tags.Where(t => t.UserId == UserId || t.UserId == Guid.Empty).ToList();
                var tags = await LoadTagsAsync();

                // Always include default tags for all users
                var defaultTags = tags.Where(t => t.IsDefault).ToList();

                // Include user-specific tags, if any
                var userTags = tags.Where(t => t.UserId == UserId).ToList();

                // Combine default and user-specific tags
                var allTags = defaultTags.Concat(userTags).ToList();

                return allTags;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Getting Tags: {ex.Message}");
                return new List<Models.Tags>();
            }
        }

    }
}
