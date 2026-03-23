using Microsoft.AspNetCore.Identity;

namespace DadJokes.Models
{
    public class Joke
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string UserId { get; set; } //user who created it
        public IdentityUser User { get; set; }

        //vote tracking
        public ICollection<Vote> Votes { get; set; }


        public Joke()
        {
            Votes = new HashSet<Vote>();
        }
    }
}
