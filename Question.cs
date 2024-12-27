using System.Collections.Generic;

namespace SemestrControlBelousovFedorov
{
    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        TextInput
    }

    public class Question
    {
        public QuestionType Type { get; set; }
        public string Text { get; set; }
        public string[] Choices { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] CorrectAnswers { get; set; }
        public string Answer { get; set; }
    }
}