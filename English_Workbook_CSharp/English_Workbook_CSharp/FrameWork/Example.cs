using System;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
    public enum exampleType
    {
        DEFAULT, WORD, IVERB, PROVERB,
    }

    public class example
    {
        private readonly string Question = "Dummy Question";
        private readonly string[] Solutions = { "Dummy Solution", };
        private readonly exampleType Type = exampleType.DEFAULT;

        public string question { get { return Question; }}
        public string[] solutions { get { return Solutions; }}
        public exampleType type { get { return Type; }}

        public example(string Question, string[] Solutions, exampleType Type)
        {
            this.Question = Question;
            this.Solutions = Solutions;
            this.Type = Type;
        }

        public static bool operator ==(example obj, example obj2) => (obj.Question == obj2.Question) && (obj.Solutions.SequenceEqual(obj2.Solutions));
        public static bool operator !=(example obj, example obj2) => (obj.Question != obj2.Question) || !(obj.Solutions.SequenceEqual(obj2.Solutions));
    }

    public class examCollections
    {
        public static List<example> Workbook = new List<example>();
        public static List<example> WrongWord = new List<example>();
        public static List<example> WrongIverb = new List<example>();
        public static List<example> WrongProverb = new List<example>();
    }
}
