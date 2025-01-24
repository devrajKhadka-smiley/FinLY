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
        //private readonly string FinLyFilePath = Path.Combine(AppContext.BaseDirectory, "FinLYDatabaseTags.json");

        private static string GetTagFilePath()
        {
            string FinLYDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Define the folder where the file will be stored
            string FinLYDatabaseFolder = Path.Combine(FinLYDocumentPath, "FinLY Database");

            // Create the directory if it does not exist
            if (!Directory.Exists(FinLYDatabaseFolder))
            {
                Directory.CreateDirectory(FinLYDatabaseFolder);
            }

            // Return the full file path for the JSON file
            //return Path.Combine(FinLYDatabaseFolder, "FinLYDatabaseTags.json");
            return Path.Combine(FinLYDatabaseFolder, "Tags Database.json");
        }


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
                string finLYTagFilePath = GetTagFilePath();

                var json = JsonSerializer.Serialize(tags, new JsonSerializerOptions { WriteIndented = true }); 
                await File.WriteAllTextAsync(finLYTagFilePath, json);
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
                string finLYTagFilePath = GetTagFilePath();

                if (!File.Exists(finLYTagFilePath))
                {
                    return new List<Models.Tags>();
                }

                var json = await File.ReadAllTextAsync(finLYTagFilePath);
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
