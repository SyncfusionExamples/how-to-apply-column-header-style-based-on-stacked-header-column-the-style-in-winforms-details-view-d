#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.Input.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace SfDataGridDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            SampleCustomization();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.sfDataGrid1.ExpandAllDetailsView();
        }

        SfDataGrid childGrid;

        /// <summary>
        /// Sets the sample customization settings.
        /// </summary>
        private void SampleCustomization()
        {
            OrderInfoRepository order = new OrderInfoRepository();
            List<OrderInfo> orderDetails = order.GetOrdersDetails(100);
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            this.sfDataGrid1.DataSource = orderDetails;

            GridViewDefinition orderDetailsView = new GridViewDefinition();
            orderDetailsView.RelationalColumn = "OrderDetails";

            childGrid = new SfDataGrid();
            childGrid.AutoGenerateColumns = false;
            childGrid.RowHeight = 21;
            childGrid.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = 0;
            nfi.NumberGroupSizes = new int[] { };
            childGrid.Columns.Add(new GridNumericColumn() { MappingName = "OrderID", HeaderText = "Order ID", NumberFormatInfo = nfi });
            childGrid.Columns.Add(new GridNumericColumn() { MappingName = "ProductID", HeaderText = "Product ID", NumberFormatInfo = nfi });
            childGrid.Columns.Add(new GridNumericColumn() { MappingName = "UnitPrice", HeaderText = "Unit Price", FormatMode = FormatMode.Currency });
            childGrid.Columns.Add(new GridNumericColumn() { MappingName = "Quantity" });
            childGrid.Columns.Add(new GridNumericColumn() { MappingName = "Discount", FormatMode = Syncfusion.WinForms.Input.Enums.FormatMode.Percent });
            childGrid.Columns.Add(new GridTextColumn() { MappingName = "CustomerID", HeaderText = "Customer ID" });
            childGrid.Columns.Add(new GridDateTimeColumn() { MappingName = "OrderDate", HeaderText = "Order Date" });

            //Creating instance for StackedHeaderRow
            StackedHeaderRow stackedHeaderRow1 = new StackedHeaderRow();

            //Adding columns to StackedColumns collection to StackedHeaderRow object.
            stackedHeaderRow1.StackedColumns.Add(new StackedColumn() { ChildColumns = "OrderID", HeaderText = "Sales Details" });

            stackedHeaderRow1.StackedColumns.Add(new StackedColumn() { ChildColumns = "ProductID", HeaderText = "Order Details" });
            stackedHeaderRow1.StackedColumns.Add(new StackedColumn() { ChildColumns = "UnitPrice", HeaderText = "Customer Details" });
            stackedHeaderRow1.StackedColumns.Add(new StackedColumn() { ChildColumns = "Quantity,Discount", HeaderText = "Product Details" });


            //Adding StackedHeaderRow object to StackedHeaderRows collection of SfDatagrid.
            childGrid.StackedHeaderRows.Add(stackedHeaderRow1);

            //trigger the Draw cell event for DetailsView DataGrid
            childGrid.DrawCell += SfDataGrid1_DrawCell;
            orderDetailsView.DataGrid = childGrid;

            this.sfDataGrid1.DetailsViewDefinitions.Add(orderDetailsView);
        }

        private void SfDataGrid1_DrawCell(object sender, Syncfusion.WinForms.DataGrid.Events.DrawCellEventArgs e)
        {
            //Get the Stakced Header Row in Details View DataGrid
            if ((e.DataRow as DataRowBase).RowType == Syncfusion.WinForms.DataGrid.Enums.RowType.StackedHeaderRow)
            {
                int columnIndex = e.ColumnIndex;

                //get the Stakced Header Column 
                if (e.CellValue == "Sales Details")
                {
                    //Apply style  to Stacked Header
                    e.Style.BackColor = Color.Yellow;

                    //check the index for avoid the Index Out range exception
                    if (childGrid.StackedHeaderRows[e.RowIndex].StackedColumns.Count == e.ColumnIndex)
                        columnIndex = e.ColumnIndex - 1;

                    //get the Child Column of specific Stacked header column
                    var childColumnName = childGrid.StackedHeaderRows[e.RowIndex].StackedColumns[columnIndex].ChildColumns.Split(',').ToList<string>();

                    foreach (var stackedColumnName in childColumnName.ToList())
                    {
                        //apply the Column Header Style based on Stacked Header child Columns
                        childGrid.Columns[stackedColumnName].HeaderStyle.BackColor = Color.Yellow;

                    }
                }

                if (e.CellValue.ToString() == "Order Details")
                {
                    //Apply style  to Stacked Header
                    e.Style.BackColor = Color.DarkCyan;
                    e.Style.TextColor = Color.White;

                    if (childGrid.StackedHeaderRows[e.RowIndex].StackedColumns.Count == e.ColumnIndex)
                        columnIndex = e.ColumnIndex - 1;

                    var childColumnName = childGrid.StackedHeaderRows[e.RowIndex].StackedColumns[columnIndex].ChildColumns.Split(',').ToList<string>();

                    foreach (var stackedColumnName in childColumnName.ToList())
                    {
                        //apply the Column Header Style based on Stacked Header child Columns
                        childGrid.Columns[stackedColumnName].HeaderStyle.BackColor = Color.DarkCyan;
                        childGrid.Columns[stackedColumnName].HeaderStyle.TextColor = Color.White;
                    }
                }
                if (e.CellValue == "Customer Details")
                {
                    e.Style.BackColor = Color.LightCyan;

                    if (childGrid.StackedHeaderRows[e.RowIndex].StackedColumns.Count == e.ColumnIndex)
                        columnIndex = e.ColumnIndex - 1;

                    var childColumnName = childGrid.StackedHeaderRows[e.RowIndex].StackedColumns[columnIndex].ChildColumns.Split(',').ToList<string>();

                    foreach (var stackedColumnName in childColumnName.ToList())
                    {
                        //apply the Column Header Style based on Stacked Header child Columns
                        childGrid.Columns[stackedColumnName].HeaderStyle.BackColor = Color.LightCyan;
                    }
                }
                if (e.CellValue == "Product Details")
                {
                    e.Style.BackColor = Color.DarkGray;
                    e.Style.TextColor = Color.White;

                    if (childGrid.StackedHeaderRows[e.RowIndex].StackedColumns.Count == e.ColumnIndex)
                        columnIndex = e.ColumnIndex - 1;
                    var childColumnName = childGrid.StackedHeaderRows[e.RowIndex].StackedColumns[columnIndex].ChildColumns.Split(',').ToList<string>();

                    foreach (var stackedColumnName in childColumnName.ToList())
                    {
                        //apply the Column Header Style based on Stacked Header child Columns
                        childGrid.Columns[stackedColumnName].HeaderStyle.BackColor = Color.DarkGray;
                        childGrid.Columns[stackedColumnName].HeaderStyle.TextColor = Color.White;
                    }
                }
            }
        }
    }
}
