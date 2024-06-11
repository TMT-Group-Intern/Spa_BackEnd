using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateOndelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Branches_BranchID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Customers_CustomerID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_EmployeeID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_ChooseServices_Appointments_AppointmentID",
                table: "ChooseServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ChooseServices_Services_ServiceID",
                table: "ChooseServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_ClientTypes_CustomerTypeID",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Branches_BranchID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobTypes_JobTypeID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_ProductID",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Sales_SaleID",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Customers_CustomerID",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Employees_EmployeeID",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Branches_BranchID",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Products_ProductID",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_BranchID",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchases",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_SaleID",
                table: "Purchases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                columns: new[] { "BranchID", "ProductID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchases",
                table: "Purchases",
                columns: new[] { "SaleID", "ProductID" });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_ProductID",
                table: "Warehouses",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductID",
                table: "Purchases",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Branches_BranchID",
                table: "Appointments",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Customers_CustomerID",
                table: "Appointments",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_EmployeeID",
                table: "Appointments",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChooseServices_Appointments_AppointmentID",
                table: "ChooseServices",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "AppointmentID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChooseServices_Services_ServiceID",
                table: "ChooseServices",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_ClientTypes_CustomerTypeID",
                table: "Customers",
                column: "CustomerTypeID",
                principalTable: "ClientTypes",
                principalColumn: "CustomerTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Branches_BranchID",
                table: "Employees",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobTypes_JobTypeID",
                table: "Employees",
                column: "JobTypeID",
                principalTable: "JobTypes",
                principalColumn: "JobTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_ProductID",
                table: "Purchases",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Sales_SaleID",
                table: "Purchases",
                column: "SaleID",
                principalTable: "Sales",
                principalColumn: "SaleID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Customers_CustomerID",
                table: "Sales",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Employees_EmployeeID",
                table: "Sales",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Branches_BranchID",
                table: "Warehouses",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Products_ProductID",
                table: "Warehouses",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Branches_BranchID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Customers_CustomerID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_EmployeeID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_ChooseServices_Appointments_AppointmentID",
                table: "ChooseServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ChooseServices_Services_ServiceID",
                table: "ChooseServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_ClientTypes_CustomerTypeID",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Branches_BranchID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobTypes_JobTypeID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_ProductID",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Sales_SaleID",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Customers_CustomerID",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Employees_EmployeeID",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Branches_BranchID",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Products_ProductID",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_ProductID",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purchases",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_ProductID",
                table: "Purchases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                columns: new[] { "ProductID", "BranchID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purchases",
                table: "Purchases",
                columns: new[] { "ProductID", "SaleID" });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_BranchID",
                table: "Warehouses",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SaleID",
                table: "Purchases",
                column: "SaleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Branches_BranchID",
                table: "Appointments",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Customers_CustomerID",
                table: "Appointments",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_EmployeeID",
                table: "Appointments",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChooseServices_Appointments_AppointmentID",
                table: "ChooseServices",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "AppointmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChooseServices_Services_ServiceID",
                table: "ChooseServices",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_ClientTypes_CustomerTypeID",
                table: "Customers",
                column: "CustomerTypeID",
                principalTable: "ClientTypes",
                principalColumn: "CustomerTypeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Branches_BranchID",
                table: "Employees",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobTypes_JobTypeID",
                table: "Employees",
                column: "JobTypeID",
                principalTable: "JobTypes",
                principalColumn: "JobTypeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_ProductID",
                table: "Purchases",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Sales_SaleID",
                table: "Purchases",
                column: "SaleID",
                principalTable: "Sales",
                principalColumn: "SaleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Customers_CustomerID",
                table: "Sales",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Employees_EmployeeID",
                table: "Sales",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Branches_BranchID",
                table: "Warehouses",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Products_ProductID",
                table: "Warehouses",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
