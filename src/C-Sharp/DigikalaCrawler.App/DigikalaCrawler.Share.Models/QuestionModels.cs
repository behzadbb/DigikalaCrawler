using System;
using System.Collections.Generic;
using System.Text;

namespace DigikalaCrawler.Share.Models.Question
{
    public class QuestionResponse
    {
        public int status { get; set; }
        public Questions data { get; set; }
    }

    public class Questions
    {
        public List<Question> questions { get; set; }
        public PagerQuestion pager { get; set; }
    }

    public class PagerQuestion
    {
        public int current_page { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
    }

    public class Question
    {
        public int id { get; set; }
        public string text { get; set; }
        public int answer_count { get; set; }
        public string sender { get; set; }
        public string created_at { get; set; }
        public List<Answer> answers { get; set; }
    }

    public class Answer
    {
        public int id { get; set; }
        public string text { get; set; }
        public QuestionReactions reactions { get; set; }
        public string created_at { get; set; }
        public string sender { get; set; }
        public string type { get; set; }
        public bool has_qa_badge { get; set; }
    }

    public class QuestionReactions
    {
        public int likes { get; set; }
        public int dislikes { get; set; }
    }

}
