﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exam_Cell
{
    public partial class examhall : Form
    {
        Connection con = new Connection();
        public examhall()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void examhall_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'exam_Cell_Rooms.Rooms' table. You can move, or remove it, as needed.
            this.roomsTableAdapter.Fill(this.exam_Cell_Rooms.Rooms);
            
            Rooms_dgv.DataSource = roomsBindingSource;
            Rooms_dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
            FillCapacity();
            Priority_combobox.SelectedIndex = 0;


        }
        void FillCapacity()
        {
            int a, b,result_a=0,result_b=0;
            foreach (DataGridViewRow dr in Rooms_dgv.Rows)
            {
               if (int.TryParse(dr.Cells["A_Series"].Value.ToString(), out a) && int.TryParse(dr.Cells["B_Series"].Value.ToString(), out b))
                {
                    result_a += a;
                    result_b += b;
                }
                
                TotalRoom_textbox.Text = Rooms_dgv.RowCount.ToString();
                TotalCapacity_textbox.Text = ("A - " + result_a + "  B - " + result_b);
            }

        }
                
        private void Save_button_Click(object sender, EventArgs e)
        {
            if(Priority_combobox.SelectedIndex!=0)
            {
                int flag = 0;
                MessageBox.Show(Rooms_dgv.RowCount.ToString());
                if (Rooms_dgv.RowCount.ToString() != "0")
                {
                    foreach (DataGridViewRow dr in Rooms_dgv.Rows)
                    {
                        if (dr.Cells["Room_No"].Value.ToString() == RoomNo_textbox.Text)
                        {
                            flag = 1;
                        }
                        
                    }
                    if(flag == 1)
                    {
                        SqlUpdateCommand();
                    }
                    else
                    {
                        SqlInsertCommand();
                    }
                }
                else
                {
                    SqlInsertCommand();
                }
            }
            else
            {
                MessageBox.Show("Select Priority", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SqlInsertCommand()
        {
            if (int.TryParse(A_series_textbox.Text, out int a) && int.TryParse(B_series_textbox.Text, out int b))
            {
                SqlCommand comm = new SqlCommand("Insert into Rooms(Room_No,Priority,A_Series,B_Series)Values(" + "@RoomNo,@Priority,@A_series,@B_series)", con.ActiveCon());
                comm.Parameters.AddWithValue("@RoomNo", RoomNo_textbox.Text);
                comm.Parameters.AddWithValue("@Priority", Priority_combobox.SelectedItem);
                comm.Parameters.AddWithValue("@A_series", a);
                comm.Parameters.AddWithValue("@B_series", b);
                comm.ExecuteNonQuery();
                MessageBox.Show("New Room Saved");
                Cleardata();
            }
            else
            {
                MessageBox.Show("A & B Series must be Numbers","Alert",MessageBoxButtons.OK,MessageBoxIcon.Error); 
            }
            
        }

        void SqlUpdateCommand()
        {
            if (int.TryParse(A_series_textbox.Text, out int a) && int.TryParse(B_series_textbox.Text, out int b))
            {
                SqlCommand comm = new SqlCommand("Update Rooms set Priority=@Priority,A_Series=@A_series,B_Series=@B_series where Room_No=@RoomNo", con.ActiveCon());
                comm.Parameters.AddWithValue("@RoomNo", RoomNo_textbox.Text);
                comm.Parameters.AddWithValue("@Priority", Priority_combobox.SelectedItem);
                comm.Parameters.AddWithValue("@A_series", a);
                comm.Parameters.AddWithValue("@B_series", b);
                comm.ExecuteNonQuery();
                MessageBox.Show(RoomNo_textbox.Text+" Updated");
                Cleardata();
            }
            else
            {
                MessageBox.Show("A & B Series must be Numbers", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        void Cleardata()
        {
            RoomNo_textbox.ResetText();
            Priority_combobox.SelectedIndex = 0;
            A_series_textbox.ResetText();
            B_series_textbox.ResetText();
            this.roomsTableAdapter.Fill(this.exam_Cell_Rooms.Rooms);
            FillCapacity();
        }
        private void TotalRoom_textbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
