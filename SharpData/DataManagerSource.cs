using AMS.Profile;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SharpData
{
	public struct DataGridManager
	{
		private DataGridView dataGrid;
		private List<DataGridViewRow> rows;
		private List<DataGridViewColumn> columns;

		public DataGridView DataGridView => dataGrid;
		public List<DataGridViewRow> Rows => rows;
		public List<DataGridViewColumn> Columns => columns;

		private DataGridViewRow FirstRow => dataGrid.Rows[0];
		private DataGridViewRow LastRow => dataGrid.Rows[dataGrid.Rows.Count - 1];

		private DataGridViewColumn FirstColumn => dataGrid.Columns[0];
		private DataGridViewColumn LastColumn => dataGrid.Columns[dataGrid.Columns.Count - 1];

		public DataGridManager(DataGridView DataGrid)
		{
			dataGrid = DataGrid;
			rows = new List<DataGridViewRow>();
			columns = new List<DataGridViewColumn>();

			for (int i = 0; i < DataGrid.Rows.Count; i++)
			{
				rows.Add(DataGrid.Rows[i]);

				for (int ii = 0; ii < DataGrid.Columns.Count; ii++)
				{
					columns.Add(DataGrid.Columns[ii]);
				}
			}
		}
		public DataGridManager(DataGridManager DataGridManager)
		{
			dataGrid = DataGridManager.DataGridView;
			rows = DataGridManager.Rows;
			columns = DataGridManager.Columns;
		}
		public DataGridManager(string iniFile)
		{
			dataGrid = new DataGridView();
			rows = new List<DataGridViewRow>();
			columns = new List<DataGridViewColumn>();

			LoadFromIni(iniFile);
		}

		public void AddColumn(int position, string columnName)
		{
			DataGridViewColumn newColumn = new DataGridViewColumn { Name = columnName, HeaderText = columnName };
			dataGrid.Columns.Insert(position, newColumn);
			columns.Insert(position, newColumn);
		}
		public void AddColumnToEnd(string columnName)
		{
			AddColumn(dataGrid.Columns.Count - 1, columnName);
		}
		public void AddColumnToBeginning(string columnName)
		{
			AddColumn(0, columnName);
		}

		public void AddRow(int position, string rowData)
		{
			DataGridViewRow newRow = new DataGridViewRow();
			newRow.CreateCells(dataGrid);
			
			for (int i = 0; i < newRow.Cells.Count; i++)
			{
				List<string> data = rowData.Split('-').ToList();
				if (data.Count < newRow.Cells.Count)
				{
					if (i > data.Count)
					{
						newRow.Cells[i].Value = string.Empty;
					}
					else
					{
						newRow.Cells[i].Value = data[i];
					}
				}
				else
				{
					newRow.Cells[i].Value = data[i];
				}
			}

			dataGrid.Rows.Insert(position, newRow);
			rows.Insert(position, newRow);
		}
		public void AddRowToEnd(string rowData)
		{
			AddRow(dataGrid.Rows.Count - 1, rowData);
		}
		public void AddRowToBeginning(string rowData)
		{
			AddRow(0, rowData);
		}

		public void RemoveColumn(int position)
		{
			dataGrid.Columns.RemoveAt(position);
		}
		public void RemoveLastColumn()
		{
			dataGrid.Columns.Remove(LastColumn);
		}
		public void RemoveFirstColumn()
		{
			dataGrid.Columns.Remove(FirstColumn);
		}

		public void RemoveRow(int position)
		{
			dataGrid.Rows.RemoveAt(position);
		}
		public void RemoveLastRow()
		{
			dataGrid.Rows.Remove(LastRow);
		}
		public void RemoveFirstRow()
		{
			dataGrid.Rows.Remove(FirstRow);
		}

		public void SetColumnName(int position, string newColumnName)
		{
			dataGrid.Columns[position].Name = newColumnName;
			dataGrid.Columns[position].HeaderText = newColumnName;
		}
		public void SetColumnName(string oldColumnName, string newColumnName)
		{
			foreach (DataGridViewColumn column in dataGrid.Columns)
			{
				if (column.Name == oldColumnName && column.HeaderText == oldColumnName)
				{
					column.Name = newColumnName;
					column.HeaderText = newColumnName;
				}
			}
		}

		public void SetCellValue(int column, int row, string newValue)
		{
			dataGrid[column, row].Value = newValue;
		}
		public void SetCellValue(string columnName, int row, string newValue)
		{
			foreach (DataGridViewColumn column in dataGrid.Columns)
			{
				if (column.Name == columnName && column.HeaderText == columnName)
				{
					int position = column.DisplayIndex;
					dataGrid[position, row].Value = newValue;
				}
			}
		}

		public void SetRowData_Array(int position, string[] data)
		{
			DataGridViewRow row = dataGrid.Rows[position];
			for (int i = 0; i < row.Cells.Count; i++)
			{
				if (data.Length < row.Cells.Count)
				{
					if (data.Length < i)
					{
						row.Cells[i].Value = string.Empty;
					}
					else
					{
						row.Cells[i].Value = data[i];
					}
				}
				else
				{
					row.Cells[i].Value = data[i];
				}
			}
		}
		public void SetRowData_List(int position, List<string> data)
		{
			SetRowData_Array(position, data.ToArray());
		}

		public string[] GetColumnData_Array(int position)
		{
			string[] strArray = new string[dataGrid.Rows.Count];

			for (int i = 0; i < dataGrid.Rows.Count; i++)
			{
				strArray[i] = dataGrid[position, i].Value.ToString();
			}

			return strArray;
		}
		public List<string> GetColumnData_List(int position)
		{
			return GetColumnData_Array(position).ToList();
		}

		public string[] GetRowData_Array(int position)
		{
			string[] strArray = new string[dataGrid.Columns.Count];

			for (int i = 0; i < dataGrid.Columns.Count; i++)
			{
				strArray[i] = dataGrid[i, position].Value.ToString();
			}

			return strArray;
		}
		public List<string> GetRowData_List(int position)
		{
			return GetRowData_Array(position).ToList();
		}

		public void SaveToIni(string iniFile)
		{
			Ini ini = new Ini(iniFile);

			for (int c = 0; c < columns.Count; c++)
			{
				ini.SetValue("Column-Names", "Column-" + c, columns[c].HeaderText);
			}

			for (int i = 0; i < dataGrid.Rows.Count; i++)
			{
				for (int ii = 0; ii < dataGrid.Rows[i].Cells.Count; ii++)
				{
					ini.SetValue("Row-" + i, "Cell-" + ii, dataGrid.Rows[i].Cells[ii].Value.ToString());
				}
			}
		}
		public void LoadFromIni(string iniFile)
		{
			Ini ini = new Ini(iniFile);

			string[] sections = ini.GetSectionNames();
			string columnSection = sections[0];
			string[] columnNames = ini.GetEntryNames(columnSection);
			string[] rows = new string[sections.Length - 1];

			for (int r = 1; r < sections.Length; r++)
			{
				rows[r] = sections[r];
			}

			DataGridView datagrid = new DataGridView();
			List<DataGridViewRow> newRows = new List<DataGridViewRow>();
			List<DataGridViewColumn> newColumns = new List<DataGridViewColumn>();

			for (int c = 0; c < columnNames.Length; c++)
			{
				string columnName = ini.GetValue(columnSection, columnNames[c]).ToString();
				DataGridViewColumn newColumn = new DataGridViewColumn { Name = columnName, HeaderText = columnName };
				newColumns.Add(newColumn);
				datagrid.Columns.Add(newColumn);
			}

			for (int i = 0; i < rows.Length; i++)
			{
				string[] cells = ini.GetEntryNames(rows[i]);
				for (int ii = 0; ii < cells.Length; ii++)
				{
					DataGridViewRow newRow = new DataGridViewRow();
					newRow.CreateCells(datagrid);
					newRow.Cells[ii].Value = ini.GetValue(rows[i], cells[ii]).ToString();

					newRows.Add(newRow);
					datagrid.Rows.Add(newRow);
				}
			}

			dataGrid = datagrid;
			this.rows = newRows;
			columns = newColumns;
		}

		public static DataGridManager CreateManager(DataGridView DataGrid)
		{
			return new DataGridManager(DataGrid);
		}
		public static DataGridManager CreateManager(DataGridManager dataGridManager)
		{
			return new DataGridManager(dataGridManager);
		}
		public static DataGridManager CreateManager(string iniFile)
		{
			return new DataGridManager(iniFile);
		}
	}
}
