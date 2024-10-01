using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konteh.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitalMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Candidates",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Candidates", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Questions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Category = table.Column<int>(type: "int", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Questions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Exams",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CandidateId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Exams", x => x.Id);
                table.ForeignKey(
                    name: "FK_Exams_Candidates_CandidateId",
                    column: x => x.CandidateId,
                    principalTable: "Candidates",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Answers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                QuestionId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Answers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Answers_Questions_QuestionId",
                    column: x => x.QuestionId,
                    principalTable: "Questions",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "ExamQuestions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                QuestionId = table.Column<int>(type: "int", nullable: false),
                ExamId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExamQuestions", x => x.Id);
                table.ForeignKey(
                    name: "FK_ExamQuestions_Exams_ExamId",
                    column: x => x.ExamId,
                    principalTable: "Exams",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ExamQuestions_Questions_QuestionId",
                    column: x => x.QuestionId,
                    principalTable: "Questions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AnswerExamQuestion",
            columns: table => new
            {
                ExamQuestionId = table.Column<int>(type: "int", nullable: false),
                SelectedAnswersId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AnswerExamQuestion", x => new { x.ExamQuestionId, x.SelectedAnswersId });
                table.ForeignKey(
                    name: "FK_AnswerExamQuestion_Answers_SelectedAnswersId",
                    column: x => x.SelectedAnswersId,
                    principalTable: "Answers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AnswerExamQuestion_ExamQuestions_ExamQuestionId",
                    column: x => x.ExamQuestionId,
                    principalTable: "ExamQuestions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AnswerExamQuestion_SelectedAnswersId",
            table: "AnswerExamQuestion",
            column: "SelectedAnswersId");

        migrationBuilder.CreateIndex(
            name: "IX_Answers_QuestionId",
            table: "Answers",
            column: "QuestionId");

        migrationBuilder.CreateIndex(
            name: "IX_ExamQuestions_ExamId",
            table: "ExamQuestions",
            column: "ExamId");

        migrationBuilder.CreateIndex(
            name: "IX_ExamQuestions_QuestionId",
            table: "ExamQuestions",
            column: "QuestionId");

        migrationBuilder.CreateIndex(
            name: "IX_Exams_CandidateId",
            table: "Exams",
            column: "CandidateId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AnswerExamQuestion");

        migrationBuilder.DropTable(
            name: "Answers");

        migrationBuilder.DropTable(
            name: "ExamQuestions");

        migrationBuilder.DropTable(
            name: "Exams");

        migrationBuilder.DropTable(
            name: "Questions");

        migrationBuilder.DropTable(
            name: "Candidates");
    }
}
