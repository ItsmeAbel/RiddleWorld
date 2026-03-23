using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DadJokes.Migrations
{
    public partial class VotesConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joke_AspNetUsers_UserId",
                table: "Joke");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Joke_JokeId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_JokeId",
                table: "Votes");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_JokeId_UserId",
                table: "Votes",
                columns: new[] { "JokeId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Joke_AspNetUsers_UserId",
                table: "Joke",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Joke_JokeId",
                table: "Votes",
                column: "JokeId",
                principalTable: "Joke",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joke_AspNetUsers_UserId",
                table: "Joke");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Joke_JokeId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_JokeId_UserId",
                table: "Votes");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_JokeId",
                table: "Votes",
                column: "JokeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Joke_AspNetUsers_UserId",
                table: "Joke",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Joke_JokeId",
                table: "Votes",
                column: "JokeId",
                principalTable: "Joke",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
