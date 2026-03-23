using Microsoft.AspNetCore.Identity;
namespace DadJokes.Models
{
    public class test
    {

        public int Id { get; set; }
        public int JokeId { get; set; }
        //public virtual Joke Joke { get; set; } //virtual meaning that it can be overriden in a child class

        public string UserId { get; set; }

        public int Value { get; set; }
    }
}
