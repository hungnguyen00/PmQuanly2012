﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using PMQuanLy.Model;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using PMQuanLy.ServiceAppCust;
using System.Xml.Linq;

namespace PMQuanLy
{
    public partial class MainForm : Form
    {
        ListProductModel mProduct;
        InventoryModel mInventory;
        OrderModel mOrder;
        OrderDetailModel mOrderDetail;
        ServiceAppCust.ServiceCustomerControllerPortTypeClient serviceAppCust;

        private DataTable dtProduct, dtInventory, dtOrder;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mProduct = new ListProductModel();
            mInventory = new InventoryModel();
            mOrder = new OrderModel();
            mOrderDetail = new OrderDetailModel();
            serviceAppCust = new ServiceCustomerControllerPortTypeClient();
            xtraTabControl1.SelectedTabPageIndex = 0;
            LoadDataProduct();
        }
        private void LoadDataProduct()
        {
            dtProduct = new DataTable();
            dtProduct = mProduct.getAllProduct();
            gvProduct.DataSource = dtProduct;
        }
        private void LoadDataInventory()
        {
            gvInventory.DataSource = dtInventory;

            //reset textbox
            txtInventoryBarCodeInsert.Text = "";
            txtInventoryWeightInsert.Text = "";

            //reset validate error
            lblInventoryBarCodeInsertError.Text = "";
            lblInventoryWeightInsertError.Text = "";
            txtInventoryBarCodeInsert.Select();
        }
        private void LoadDataOrder()
        {

            // This line of code is generated by Data Source Configuration Wizard
            // Fill a SqlDataSource
            sqlDSInventory.Fill();

            //inventory
            dtInventory = new DataTable();
            dtInventory = mInventory.getAllInventory();
            dtInventory.Columns.Add("quantity", typeof(int));
            gvOrderOrder.DataSource = dtInventory;

            //order
            dtOrder = new DataTable();
            dtOrder.Columns.Add("name");
            dtOrder.Columns.Add("product_code");
            dtOrder.Columns.Add("qr_code");
            dtOrder.Columns.Add("weight");
            dtOrder.Columns.Add("created_date");
            gvOrderOrderInventory.DataSource = dtOrder;

            //product
            dtProduct = new DataTable();
            dtProduct.Columns.Add("name");
            dtProduct.Columns.Add("quantity", typeof(int));
            dtProduct.Columns.Add("product_code");
            dtProduct.Columns.Add("weight",typeof(float));
            gvOrderOrderProduct.DataSource = dtProduct;

            //set point focus
            txtOrderQrCode.Select();
        }

        private void LoadDataOrderList()
        {
            //order
            dtOrder = new DataTable();
            dtOrder = mOrder.getAllOrder();
            gvOrderListOrder.DataSource = dtOrder;
        }

        private void resetInventoryGridControl()
        {
            dtInventory = new DataTable();
            dtInventory.Columns.Add("product_name");
            dtInventory.Columns.Add("product_code");
            dtInventory.Columns.Add("qr_code");
            dtInventory.Columns.Add("weight");
        }
        private void navBarProduct_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 0;
            LoadDataProduct();
        }

        private void navBarProductDetail_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 1;
            resetInventoryGridControl();
            LoadDataInventory();
        }
        private void navBarOrder_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 2;
            LoadDataOrder();
        }
        private void navBarOrderList_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            xtraTabControl1.SelectedTabPageIndex = 3;
            LoadDataOrderList();
        }
        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                txtOrderQrCode.Text = view.GetRowCellValue(view.GetSelectedRows()[0], "qr_code").ToString();
            }
        }
        private void orderWithQrCode(String qr_code) 
        {
            //get value to order inventory gridcontrol & delete value on inventory grid control
            DataRow[] arrRowInventory = dtInventory.Select(String.Format("qr_code = '{0}'",qr_code));

            //add new row to order inventory grid control
            dtOrder.ImportRow(arrRowInventory[0]);
            gvOrderOrderInventory.RefreshDataSource();

            //get value to product gridcontrol
            string[] arrCode = qr_code.Split('.');
            string product_code = arrCode[0];
            bool product_code_found = false;
            for (int i = 0; i < dtProduct.Rows.Count; i++) 
            {
                if (dtProduct.Rows[i]["product_code"].Equals(product_code)) 
                {
                    float weight = float.Parse(dtProduct.Rows[i]["weight"].ToString()) + float.Parse(arrRowInventory[0]["weight"].ToString());
                    dtProduct.Rows[i]["weight"] = weight.ToString();
                    int quantity = int.Parse(dtProduct.Rows[i]["quantity"].ToString()) + 1;
                    dtProduct.Rows[i]["quantity"] = quantity.ToString();
                    product_code_found = true;
                }
            }
            if (!product_code_found)
            {
                arrRowInventory[0]["quantity"] = "1";
                dtProduct.ImportRow(arrRowInventory[0]);
            }
            //update row on product grid control
            gvOrderOrderProduct.RefreshDataSource();

            //delete row on inventory grid control
            dtInventory.Rows.Remove(arrRowInventory[0]);
            gvOrderOrder.RefreshDataSource();

            //get sum_weight + sum_quantity
            calculateSumWeight(1);
        }
        private void calculateSumWeight(int page)
        {
            String sum_weight = "0";
            String sum_quantity = "0";

            //get sum weight
            sum_weight = dtProduct.Compute("SUM(weight)", String.Empty).ToString();
            //get sum quantity
            sum_quantity = dtProduct.Compute("SUM(quantity)", String.Empty).ToString();

            switch (page)
            {
                case 1: //order page
                    lblOrderSumWeight.Text = sum_weight;
                    lblOrderSumQuantity.Text = sum_quantity;
                    break;
                default:
                    break;
            }

        }

        private void txtQrCode_TextChanged(object sender, EventArgs e)
        {
            if (txtOrderQrCode.Text.Length == 27) 
            {
                orderWithQrCode(txtOrderQrCode.Text.ToString());
            }
        }

        private void txtOrderSearchProductName_TextChanged(object sender, EventArgs e)
        {
            customFilterInventory();
        }

        private void txtOrderSearchProductCode_TextChanged(object sender, EventArgs e)
        {
            customFilterInventory();
        }

        private void customFilterInventory() 
        {

            String key1 = "name",
                   key2 = "product_code";
            String value1 = txtOrderSearchProductName.Text.ToString(),
                   value2 = txtOrderSearchProductCode.Text.ToString();

            String filterString = "1 = 1 ";

            if (!value1.Equals(""))
            {
                filterString += String.Format(" AND [{0}] LIKE '%{1}%'", key1, value1);
            }
            if (!value2.Equals(""))
            {
                filterString += String.Format(" AND [{0}] LIKE '%{1}%'", key2, value2);
            }
            gridOrderInventory.ActiveFilterString = filterString;
        }

        private void customFilterOrderList()
        {

            String key1 = "order_id",
                   key2 = "created_date";
            String value1 = txtOrderListSearchOrderId.Text.ToString(),
                   value2 = deOrderListSearchDateFrom.Text.ToString(),
                   value3 = deOrderListSearchDateTo.Text.ToString();
            MessageBox.Show(value3);
            String filterString = "1 = 1 ";

            if (!value1.Equals(""))
            {
                filterString += String.Format(" AND [{0}] LIKE '%{1}%'", key1, value1);
            }
            if (!value2.Equals(""))
            {
                filterString += String.Format(" AND [{0}] >= #{1}#", key2, Convert.ToDateTime(value2));
            }
            if (!value3.Equals(""))
            {
                filterString += String.Format(" AND [{0}] <= #{1}#", key2, Convert.ToDateTime(value3));
            }
            gridOrderListOrder.ActiveFilterString = filterString;
        }

        private void btnOrderInventoryDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Would you like to delete this record ?",
            "Important Question",
            MessageBoxButtons.YesNo);

            //delete when clicked to yes button on message box
            if (result == DialogResult.Yes)
            {

                DataRow[] row = new DataRow[gridOrderOrderInventory.SelectedRowsCount];
                row[0] = gridOrderOrderInventory.GetDataRow(gridOrderOrderInventory.GetSelectedRows()[0]);

                //update inventory gridcontrol
                dtInventory.ImportRow(row[0]);
                gvOrderOrder.RefreshDataSource();

                //update product gridcontrol
                string product_code = row[0]["product_Code"].ToString();
                for (int i = 0; i < dtProduct.Rows.Count; i++)
                {
                    if (dtProduct.Rows[i]["product_code"].Equals(product_code))
                    {
                        float weight = float.Parse(dtProduct.Rows[i]["weight"].ToString()) - float.Parse(row[0]["weight"].ToString());
                        int quantity = int.Parse(dtProduct.Rows[i]["quantity"].ToString()) - 1;
                        if (weight.ToString() == "0") //delete
                        {
                            dtProduct.Rows.RemoveAt(i);
                        }
                        else //update
                        {
                            dtProduct.Rows[i]["weight"] = weight.ToString();
                            dtProduct.Rows[i]["quantity"] = quantity.ToString();
                        }

                    }
                }

                gvOrderOrderProduct.RefreshDataSource();

                //delete order inventory gridcontrol
                dtOrder.Rows.Remove(row[0]);
                gvOrderOrderInventory.RefreshDataSource();

                //get sum_weight + sum_quantity
                calculateSumWeight(1);
            }
        }

        private void btnOrderProductDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Would you like to delete this record ?",
            "Important Question",
            MessageBoxButtons.YesNo);

            //delete when clicked to yes button on message box
            if (result == DialogResult.Yes)
            {
                DataRow[] rowProductSelected = new DataRow[1];
                rowProductSelected[0] = gridOrderOrderProduct.GetDataRow(gridOrderOrderProduct.GetSelectedRows()[0]);
                string product_code = rowProductSelected[0]["product_Code"].ToString();
                List<DataRow> list = new List<DataRow>();
                for (int i = 0; i < dtOrder.Rows.Count; i++)
                {
                    if (dtOrder.Rows[i]["product_code"].Equals(product_code))
                    {
                        DataRow[] rowInventorySelected = new DataRow[1];
                        rowInventorySelected[0] = gridOrderOrderInventory.GetDataRow(i);
                        list.Add(rowInventorySelected[0]);
                    }
                }

                //update inventory + order inventory gridcontrol
                foreach (DataRow dr in list)
                {
                    dtInventory.ImportRow(dr);
                    dtOrder.Rows.Remove(dr);
                }
                gvOrderOrder.RefreshDataSource();
                gvOrderOrderInventory.RefreshDataSource();

                //delete product gridcontrol
                dtProduct.Rows.RemoveAt(gridOrderOrderProduct.FocusedRowHandle);
                gvOrderOrderProduct.RefreshDataSource();

                //get sum_weight + sum_quantity
                calculateSumWeight(1);
            }
        }

        private void btnOrderSubmit_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Order Successfully!!!");
            saveOrderToDB();
            LoadDataOrder();
        }
        private void saveOrderToDB()
        {
            Hashtable htInput = new Hashtable();
            htInput.Add("total_quantity", lblOrderSumQuantity.Text.ToString());
            htInput.Add("total_weight", lblOrderSumWeight.Text.ToString());
            mOrder.saveOrder((DataTable)gvOrderOrderInventory.DataSource, htInput);
        }

        private void btnInventoryInsertNew_Click(object sender, EventArgs e)
        {
            if(!validateInventoryInsert())
            {
                InsertNewInventory();
            }
        }
        private bool validateInventoryInsert()
        {
            bool result = false;
            float f;
            lblInventoryBarCodeInsertError.Text = "";
            lblInventoryWeightInsertError.Text = "";
            if(string.IsNullOrEmpty(txtInventoryWeightInsert.Text))
            {
                lblInventoryWeightInsertError.Text = "Weight can not null.";
                result = true;
            }
            else if (!float.TryParse(txtInventoryWeightInsert.Text, out f))
            {
                lblInventoryWeightInsertError.Text = "Weight is number format.";
                result = true;
            }

            //validate qr_code
            string[] seperator = {"."};
            string[] arrQrCode = txtInventoryBarCodeInsert.Text.ToString().Split(seperator, StringSplitOptions.None);

            if(!mProduct.checkProductByProductCode(arrQrCode[0]))
            {
                lblInventoryBarCodeInsertError.Text = "Product is not exist.";
                result = true;
            }
            else if (mInventory.checkInventoryByQrCode(txtInventoryBarCodeInsert.Text.ToString()))
            {
                lblInventoryBarCodeInsertError.Text = "Inventory existed.";
                result = true;
            }

            return result;
        }
        private void InsertNewInventory()
        {

            //validate qr_code
            string[] seperator = { "." };
            string[] arrQrCode = txtInventoryBarCodeInsert.Text.ToString().Split(seperator, StringSplitOptions.None);
            DataTable dtTemp = mProduct.getProductByProductCode(arrQrCode[0]);


            DataRow drTemp = dtInventory.NewRow();
            drTemp["product_name"] = dtTemp.Rows[0]["name"];
            drTemp["product_code"] = dtTemp.Rows[0]["product_code"];
            drTemp["qr_code"] = txtInventoryBarCodeInsert.Text.ToString();
            drTemp["weight"] = txtInventoryWeightInsert.Text.ToString();
            dtInventory.Rows.Add(drTemp);

            LoadDataInventory();
        }

        private void txtOrderListSearchOrderId_TextChanged(object sender, EventArgs e)
        {
            customFilterOrderList();
        }

        private void deOrderListSearchDateFrom_TextChanged(object sender, EventArgs e)
        {
            customFilterOrderList();
        }

        private void deOrderListSearchDateTo_TextChanged(object sender, EventArgs e)
        {
            customFilterOrderList();
        }
        private void gridOrderListOrder_Click(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                String order_id = view.GetRowCellValue(view.GetSelectedRows()[0], "order_id").ToString();

                dtInventory = new DataTable();
                dtInventory = mOrderDetail.getOrderDetailByOrderId(order_id);
                gvOrderListOrderInventory.DataSource = dtInventory;

                //product
                dtProduct = new DataTable();
                dtProduct = mOrderDetail.getOrderProductDetailByOrderId(order_id);
                gvOrderListOrderProduct.DataSource = dtProduct;
            }
        }

        private void btnInventorySubmit_Click(object sender, EventArgs e)
        {
            mInventory.insertInventoryList((DataTable)gvInventory.DataSource);
            if (!gridInventory.IsEmpty)
            {
                MessageBox.Show("Successfully!!!");
            }
            resetInventoryGridControl();
            LoadDataInventory();
        }

        private void btnProductSync_Click(object sender, EventArgs e)
        {
            string products = serviceAppCust.listProduct(9);
            XElement newxml = XElement.Parse(products);
            Hashtable hProduct = new Hashtable();
            var product = from element in newxml.Elements("san_pham") select element;
            foreach (var item in product)
            {
                hProduct.Clear();
                hProduct.Add("product_code", item.Attribute("ma_sp").Value);
                hProduct.Add("name", item.Attribute("ten_sp").Value);
                hProduct.Add("quantity", 0);
                hProduct.Add("total_weight", 0);

                if (!mProduct.insertNewProduct(hProduct))
                {
                    //need to log when error occur later
                }
            }
            LoadDataProduct();
        }
    }
}
