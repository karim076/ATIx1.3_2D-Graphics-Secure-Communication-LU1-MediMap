using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migraion1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Patient_PatienId",
                schema: "MediMap",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "PatienId",
                schema: "MediMap",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Patient_PatienId",
                schema: "MediMap",
                table: "User",
                column: "PatienId",
                principalSchema: "MediMap",
                principalTable: "Patient",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Patient_PatienId",
                schema: "MediMap",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "PatienId",
                schema: "MediMap",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Patient_PatienId",
                schema: "MediMap",
                table: "User",
                column: "PatienId",
                principalSchema: "MediMap",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
