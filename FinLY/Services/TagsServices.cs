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
                    tags.Add(tag);  
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

                var tags = await LoadTagsAsync();

                var defaultTags = tags.Where(t => t.IsDefault).ToList();

                var userTags = tags.Where(t => t.UserId == UserId).ToList();

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
