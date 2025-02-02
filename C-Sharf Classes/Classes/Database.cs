﻿/**
 * __________________________________________________________________
 *
 * C-Sharf Custom Classes
 * __________________________________________________________________
 *
 * MIT License
 * 
 * Copyright (c) 2020 Wilfred V. Pine
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 * @package C-Sharf Custom Classes
 * @author Wilfred V. Pine <only.master.red@gmail.com>
 * @copyright Copyright 2020 (https://red.confired.com)
 * @link https://confired.com
 * @license https://opensource.org/licenses/MIT MIT License
 */

using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace C_Sharf_Classes.Classes
{
    class Database : Config
    {
        // instantiate public variable
        Public_variables public_vars = new Public_variables();

        // connection string
        private static string constring = "server=localhost;uid=" + dbuser + ";pwd='" + dbpassword + "';database=" + dbname;

        // MySql Classes
        public MySqlConnection con = new MySqlConnection(constring);
        MySqlCommand cmd;
        MySqlDataReader reader;
        MySqlDataAdapter da;
        DataTable dt;

        // Check Database Connection
        public Database()
        {
            con = new MySqlConnection(constring);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        // select or search = return a row
        public MySqlDataReader select(string qry)
        {

            con = new MySqlConnection(constring);
            con.Open();

            cmd = new MySqlCommand(qry, con);
            reader = cmd.ExecuteReader();
            return reader;

        }

        // create / update / delete
        public void cud(string qry, string msg = "")
        {

            con = new MySqlConnection(constring);
            con.Open();

            cmd = new MySqlCommand(qry, con);
            if (cmd.ExecuteNonQuery() > 0)
            {
                if (msg == "")
                    MessageBox.Show("Transaction Completed!");
                else
                    MessageBox.Show(msg);
            }
            else
            {
                MessageBox.Show("Sorry! There's something wrong.");
            }

        }

        // Save
        public void save(string table, string[] column, string[] bind)
        {

            con = new MySqlConnection(constring);
            con.Open();

            string fields = "";
            foreach (string col in column)
            {
                fields = fields + col + ',';
            }

            string value = "";
            foreach (string val in bind)
            {
                value = value + "'" + val + "',";
            }

            var qry = "INSERT INTO " + table + "(" + fields.TrimEnd(',') + ") VALUES(" + value.TrimEnd(',') + ")";

            cmd = new MySqlCommand(qry, con);
            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Successfully saved.");
            }
            else
            {
                MessageBox.Show("Sorry! There's something wrong.");
            }
        }

        // datasource / select rows / for DataGridView
        public void table(string qry, DataGridView dgv, string[] header = null)
        {
            
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            con = new MySqlConnection(constring);
            con.Open();

            da = new MySqlDataAdapter(qry, con);
            dt = new DataTable();
            da.Fill(dt);
            dgv.DataSource = dt;

            if (header != null)
            {
                foreach (DataGridViewColumn dgvcolumn in dgv.Columns)
                {
                    dgvcolumn.HeaderText = header[dgvcolumn.Index];
                }
            }

        }

        // select & add to combo box
        public void list(string qry, ComboBox comboBox)
        {
            con = new MySqlConnection(constring);
            con.Open();

            cmd = new MySqlCommand(qry, con);
            reader = cmd.ExecuteReader();

            comboBox.Items.Clear();
            while (reader.Read())
            {
                comboBox.Items.Add(reader[0].ToString());
                comboBox.AutoCompleteCustomSource.Add(reader[0].ToString());
            }

        }

        // check data if exist
        public bool exist(string qry)
        {

            con = new MySqlConnection(constring);
            con.Open();
            
            cmd = new MySqlCommand(qry, con);
            reader = cmd.ExecuteReader();
            if (reader.Read())
                return true;
            else
                return false;

        }

        //MAx ID
        public void maxid(String column, String tbl)
        {
            con = new MySqlConnection(constring);
            con.Open();

            var cmd = new MySqlCommand("SELECT MAX(" + column + ") from " + tbl + "", con);

            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                try
                {
                    public_vars.maxid = reader.GetInt32(0) + 1;
                }
                catch (Exception)
                {
                    public_vars.maxid = 1;
                }
            }
        }


    }
}
