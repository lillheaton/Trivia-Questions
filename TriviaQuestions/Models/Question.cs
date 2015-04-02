using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaQuestions.Models
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public int Year { get; set; }
        public int Difficulty { get; set; }
        public string AirDate { get; set; }
        public virtual Category Category { get; set; }
    }
}