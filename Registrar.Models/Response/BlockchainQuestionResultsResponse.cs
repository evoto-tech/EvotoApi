using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Registrar.Models.Response
{
    [DataContract]
    public class BlockchainQuestionResultsResponse
    {
        public BlockchainQuestionResultsResponse(int number, string question, Dictionary<string, int> answers)
        {
            Number = number;
            Question = question;
            Answers = answers;
        }

        [DataMember(Name = "number")]
        public int Number { get; private set; }

        [DataMember(Name = "question")]
        public string Question { get; private set; }

        [DataMember(Name = "answers")]
        public IDictionary<string, int> Answers { get; private set; }
    }
}