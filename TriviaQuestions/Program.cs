using System.Runtime.InteropServices;

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

using TriviaQuestions.Models;

namespace TriviaQuestions
{
    public class Program
    {
        private const string Url = "http://j-archive.com";
        private const int NumberOfSeasons = 1;

        private static WebClient client = new WebClient();

        public static void Main(string[] args)
        {
            var seasonHtmlDocuments = DownloadSeasonDocuments().AsParallel();
            var gameIds = GetAllGameIds(seasonHtmlDocuments);
            var gameDocuments = DownloadGameDocuments(gameIds);
            ScrapeData(gameDocuments);


            Console.ReadKey();
            client.Dispose();
        }

        private static IEnumerable<HtmlDocument> DownloadSeasonDocuments()
        {
            for (int i = 1; i < NumberOfSeasons + 1; i++)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(client.DownloadString(string.Format("{0}/showseason.php?season={1}", Url, i)));
                yield return doc;
            }
        }

        private static IEnumerable<string> GetAllGameIds(IEnumerable<HtmlDocument> documents)
        {
            foreach (var htmlDocument in documents)
            {
                var gameLinks = htmlDocument.DocumentNode.SelectNodes("//table//td/a");
                foreach (var gameLink in gameLinks)
                {
                    yield return
                        gameLink.Attributes["href"].Value.Split(
                            new[] { "id=" },
                            StringSplitOptions.RemoveEmptyEntries)[1];
                }
            }
        }

        public static IEnumerable<HtmlDocument> DownloadGameDocuments(IEnumerable<string> gameIds)
        {
            foreach (var gameId in gameIds)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(client.DownloadString(string.Format("{0}/showgame.php?game_id={1}", Url, gameId)));
                yield return doc;
            }
        }

        public static void ScrapeData(IEnumerable<HtmlDocument> gameDocuments)
        {
            using (var db = new TriviaDbContext()) 
            {
                foreach (var gameDocument in gameDocuments)
                {
                    var airDate =
                        gameDocument.DocumentNode.SelectNodes("//div[@id='game_title']/h1")
                            .First()
                            .InnerText.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1];
                    Console.WriteLine("Working on {0}", airDate);
                    var year = int.Parse(airDate.Split(',')[2]);


                    int temp = 0;
                    var rounds = gameDocument.DocumentNode.SelectNodes("//table[@class='round']");
                    foreach (var round in rounds)
                    {
                        var questionsNodes = round.SelectNodes(round.XPath + "//td[@class='clue']");
                        var categories = round.SelectNodes(round.XPath + "//td[@class='category_name']").Select(s => s.InnerText.ToLower()).ToList();

                        foreach (var node in questionsNodes)
                        {
                            var questionDbModel = new Question();
                            questionDbModel.AirDate = airDate;
                            questionDbModel.Year = year;

                            var clueValueElement = node.SelectNodes(node.XPath + "//td[@class='clue_value']");
                            if (clueValueElement == null)
                            {
                                continue;
                            }                            

                            questionDbModel.Difficulty = int.Parse(Regex.Match(clueValueElement.First().InnerText, @"[0-9\.]+").Value);

                            var onMouseOverValue = HttpUtility.HtmlDecode(node.SelectNodes(node.XPath + "//div").First().Attributes["onmouseover"].Value);
                            questionDbModel.Answer = Regex.Match(onMouseOverValue, @"ponse"">(.*)<\/e").Groups[1].Value;
                            questionDbModel.Text = node.SelectNodes(node.XPath + "//td[@class='clue_text']").First().InnerText;

                            if (db.Questions.FirstOrDefault(s => s.Text == questionDbModel.Text) != null)
                            {
                                // question already exist
                                continue;
                            }

                            // Ugly trick to get category index
                            var index = int.Parse(Regex.Match(node.XPath.Split('/').Last(), @"\[(.*?)\]").Groups[1].Value);
                            var category = categories[temp + index - 1];

                            var dbCategory = db.Categories.FirstOrDefault(s => s.Name.ToLower() == category);
                            if (dbCategory == null)
                            {
                                dbCategory = new Category { Name = category };
                                db.Categories.Add(dbCategory);
                            }

                            questionDbModel.Category = dbCategory;
                            db.Questions.Add(questionDbModel);
                            db.SaveChanges();

                            temp += 5;
                        }
                    }
                }
            }
        }
    }
}