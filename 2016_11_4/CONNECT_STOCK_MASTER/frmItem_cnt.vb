Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Imports System.IO
Public Class frmItem_cnt
    Inherits System.Windows.Forms.Form
    Dim dsUser As DataSet
    Dim Clicked As String
    Dim c_dataCustomer1 As DataTable
    Dim _Supplier As String
    Dim _Comcode As String
    Dim _cat_Code As String

    Function Search_Supplier() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M07Sup_Code from M07Supplier where M07Status='A' and M07Sup_Name='" & Trim(cboSupplier.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Supplier = True
                _Supplier = Trim(M01.Tables(0).Rows(0)("M07Sup_Code"))
            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function


    Function Search_Category() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            If Trim(txtType.Text) = "PRODUCT ITEM" Then
                Sql = "select M09Cat_Code from M09Product_Category where M09Status='A' and M09Cat_Name='" & Trim(cboCategory.Text) & "'"
                M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
                If isValidDataset(M01) Then
                    Search_Category = True
                    _cat_Code = Trim(M01.Tables(0).Rows(0)("M09Cat_Code"))

                End If
            ElseIf Trim(txtType.Text) = "ROW ITEMS" Then

                Sql = "select M10Row_Code from M10Row_Category where M10Status='A' and M10Row_Name='" & Trim(cboCategory.Text) & "'"
                M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
                If isValidDataset(M01) Then
                    Search_Category = True
                    _cat_Code = Trim(M01.Tables(0).Rows(0)("M10Row_Code"))

                End If
            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Function Clear_Text()
        Me.txtCode.Text = ""
        Me.txtName.Text = ""
        Me.txtPic1.Text = ""
        Me.txtPic2.Text = ""
        Me.txtRemark.Text = ""
        Me.txtReorder.Text = ""
        Me.txtType.Text = ""
        Me.cboCategory.Text = ""
        Me.cboSupplier.Text = ""
        Me.txtRate.Text = ""
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
    End Function
    Function Load_Supplier()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M07Sup_Name as [##] from M07Supplier where M07Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboSupplier
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 287
                '  .Rows.Band.Columns(1).Width = 160


            End With

            With cboSupplier_1
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 247
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Category()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M09Cat_Name as [##] from M09Product_Category where M09Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCategory
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 203
                '  .Rows.Band.Columns(1).Width = 160


            End With

            'With cboDistrict_1
            '    .DataSource = M01
            '    .Rows.Band.Columns(0).Width = 258
            '    '  .Rows.Band.Columns(1).Width = 160


            'End With
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function LOAD_CATEGORY_1()

        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M10Row_Name as [##] from M10Row_Category where M10Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCategory
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 203
                '  .Rows.Band.Columns(1).Width = 160


            End With

            'With cboDistrict_1
            '    .DataSource = M01
            '    .Rows.Band.Columns(0).Width = 258
            '    '  .Rows.Band.Columns(1).Width = 160


            'End With
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Find_CATEGORY()

        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M09Cat_Name as [##] from View_Category  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCategory_1
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 247
                '  .Rows.Band.Columns(1).Width = 160


            End With

            'With cboDistrict_1
            '    .DataSource = M01
            '    .Rows.Band.Columns(0).Width = 258
            '    '  .Rows.Band.Columns(1).Width = 160


            'End With
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Private Sub txtCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCode.KeyUp
        If e.KeyCode = 13 Then
            If Trim(txtCode.Text) <> "" Then
                cboSupplier.ToggleDropdown()
            Else

            End If
        End If
    End Sub


    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Call Clear_Text()
        Call Load_Grid()
        OPR0.Visible = False
        Panel2.Visible = False
        Panel1.Visible = False
        Panel5.Visible = False
        txtCode.Focus()
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call Clear_Text()
        txtCode.Focus()
    End Sub

    Private Sub UsingDateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UsingDateToolStripMenuItem.Click
        Call Clear_Text()
        OPR0.Visible = True
        Panel2.Visible = False
        Panel1.Visible = False
        Panel5.Visible = False
        txtCode.Focus()
    End Sub

    Private Sub frmItem_cnt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_Supplier()
        Call lOAD_TYPE()
        Call Load_Grid()
        txtRate.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtReorder.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
    End Sub

    Function lOAD_TYPE()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M22Description as [##] from M22Common where M22Ref='1'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With txtType
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 119
                '  .Rows.Band.Columns(1).Width = 160


            End With


            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Private Sub cboSupplier_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboSupplier.KeyUp
        If e.KeyCode = 13 Then
            txtType.ToggleDropdown()

        End If
    End Sub

    Private Sub txtType_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtType.AfterCloseUp
        If Trim(txtType.Text) = "PRODUCT ITEM" Then
            Call Load_Category()
        Else
            Call LOAD_CATEGORY_1()
        End If
    End Sub

    Private Sub txtType_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtType.KeyUp
        If e.KeyCode = 13 Then
            If Trim(txtType.Text) = "PRODUCT ITEM" Then
                Call Load_Category()
            Else
                Call LOAD_CATEGORY_1()
            End If
            cboCategory.ToggleDropdown()
        End If
    End Sub

    Private Sub cboCategory_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCategory.KeyUp
        If e.KeyCode = 13 Then
            txtName.Focus()
        End If
    End Sub

    Private Sub txtName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyUp
        If e.KeyCode = 13 Then
            txtRemark.Focus()
        End If
    End Sub

    Private Sub txtRemark_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRemark.KeyUp
        If e.KeyCode = Keys.F1 Then
            txtReorder.Focus()
        End If
    End Sub

    Private Sub txtReorder_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtReorder.KeyUp
        If e.KeyCode = 13 Then
            txtRate.Focus()
        End If
    End Sub

    Private Sub txtRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRate.KeyUp
        Dim Value As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtRate.Text) Then
                Value = txtRate.Text
                txtRate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
            End If
            cmdAdd.Focus()
        End If
    End Sub


    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If Trim(txtCode.Text) <> "" Then
        Else
            MsgBox("Please enter the item code", MsgBoxStyle.Information, "Information ........")
            txtCode.Focus()
            Exit Sub
        End If

        If Search_Supplier() = True Then
        Else
            MsgBox("Please select the Supplier Name", MsgBoxStyle.Information, "Information ........")
            cboSupplier.ToggleDropdown()
            Exit Sub
        End If

        If Search_Category() = True Then
        Else
            MsgBox("Please select the Category Name", MsgBoxStyle.Information, "Information ........")
            cboCategory.ToggleDropdown()
            Exit Sub
        End If


        If Trim(txtType.Text) <> "" Then
        Else
            MsgBox("Please select the Item Type", MsgBoxStyle.Information, "Information ........")
            txtType.ToggleDropdown()
            Exit Sub
        End If

        If Trim(txtName.Text) <> "" Then
        Else
            MsgBox("Please enter the Description", MsgBoxStyle.Information, "Information ........")
            txtName.Focus()
            Exit Sub
        End If

        If txtRemark.Text <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        If txtReorder.Text <> "" Then
        Else
            txtReorder.Text = "0"
        End If

        If IsNumeric(txtReorder.Text) Then
        Else
            MsgBox("Please enter the correct Reorder Level", MsgBoxStyle.Information, "Information .........")
            txtReorder.Focus()
            Exit Sub
        End If

        If IsNumeric(txtRate.Text) Then
        Else
            MsgBox("Please enter the correct Rate", MsgBoxStyle.Information, "Information .........")
            txtRate.Focus()
            Exit Sub
        End If

        
        If txtPic1.Text <> "" Then
        Else
            MsgBox("Please select the Image 01", MsgBoxStyle.Information, "Information .........")
            Exit Sub
        End If

        If txtPic2.Text <> "" Then
        Else
            MsgBox("Please select the Image 02", MsgBoxStyle.Information, "Information .........")
            Exit Sub
        End If

        Call Save_Data()
    End Sub
    Function Deactivate_Data()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean

        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Try

            nvcFieldList1 = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(txtCode.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "update M11Product_Item set M11Status='I' where M11Part_No='" & Trim(txtCode.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                               " values('NEW_ITEM','DELETE', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            Else

                MsgBox("This Item Can't be deactivated", MsgBoxStyle.Information, "Information .........")
                connection.Close()
                Exit Function
                

            End If

            transaction.Commit()
            connection.Close()
            Call Clear_Text()
            Call Load_Grid()


        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function

    Function Save_Data()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean

        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim _GetDate As DateTime

        Try
            _GetDate = Month(Now) & "/" & Microsoft.VisualBasic.Day(Now) & "/" & Year(Now) & " " & Hour(Now) & ":" & Minute(Now)

            nvcFieldList1 = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(txtCode.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "update M11Product_Item set M11Cat_Code='" & _cat_Code & "',M11Type='" & Trim(txtType.Text) & "',M11Supp_Code='" & _Supplier & "',M11Part_Name='" & Trim(txtName.Text) & "',M11Re_Order=" & CInt(txtReorder.Text) & ",M11Retail_price='" & Trim(txtRate.Text) & "',M11Remark='" & Trim(txtRemark.Text) & "',M11Status='A' where M11Part_No='" & Trim(txtCode.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                               " values('NEW_ITEM','EDIT', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
            Else
                

                nvcFieldList1 = "Insert Into M11Product_Item(M11Part_No,M11Cat_Code,M11Type,M11Supp_Code,M11Part_Name,M11Re_Order,M11Retail_price,M11Remark,M11Status)" & _
                                                                  " values('" & Trim(txtCode.Text) & "','" & _cat_Code & "', '" & Trim(txtType.Text) & "','" & _Supplier & "','" & Trim(txtName.Text) & "','" & Trim(txtReorder.Text) & "','" & Trim(txtRate.Text) & "','" & Trim(txtRemark.Text) & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                " values('NEW_ITEM','SAVE', '" & _GetDate & "','" & strDisname & "','" & txtCode.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            End If
            MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.Close()
            Call Update_Image()
            

            Me.txtCode.Text = ""
            Me.txtName.Text = ""
            Me.txtRemark.Text = ""
            Me.txtReorder.Text = ""
            Me.txtRate.Text = ""
            Call Load_Grid()
            txtCode.Focus()
            'Call Load_Grid()
            'Call Load_Entry()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function
    Function Load_Grid()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11Id) ##,M11Part_No as [Item Code],M07Sup_Name as [Supplier Name],M09Cat_Name as [Category],M11Part_Name as [Item Name],CAST(M11Retail_price AS DECIMAL(16,2)) as Rate from M11Product_Item inner join M07Supplier on M07Sup_Code=M11Supp_Code inner join view_category on M09Cat_Code=M11Cat_Code where M11Status='A' order by M11id"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 40
            UltraGrid1.Rows.Band.Columns(1).Width = 90
            UltraGrid1.Rows.Band.Columns(2).Width = 120
            UltraGrid1.Rows.Band.Columns(3).Width = 120
            UltraGrid1.Rows.Band.Columns(4).Width = 260
            UltraGrid1.Rows.Band.Columns(5).Width = 120
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_item()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11Part_No) ##,M11Part_No as [Item Code],M07Sup_Name as [Supplier Name],M09Cat_Name as [Category],M11Part_Name as [Item Name],CAST(M11Retail_price AS DECIMAL(16,2)) as Rate from M11Product_Item inner join M07Supplier on M07Sup_Code=M11Supp_Code inner join view_category on M09Cat_Code=M11Cat_Code where M11Status='A' and M11Part_Name like '%" & Trim(txtName_1.Text) & "%' order by M11id"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 40
            UltraGrid1.Rows.Band.Columns(1).Width = 90
            UltraGrid1.Rows.Band.Columns(2).Width = 120
            UltraGrid1.Rows.Band.Columns(3).Width = 120
            UltraGrid1.Rows.Band.Columns(4).Width = 260
            UltraGrid1.Rows.Band.Columns(5).Width = 120
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_supplier()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11Part_No) ##,M11Part_No as [Item Code],M07Sup_Name as [Supplier Name],M09Cat_Name as [Category],M11Part_Name as [Item Name],CAST(M11Retail_price AS DECIMAL(16,2)) as Rate from M11Product_Item inner join M07Supplier on M07Sup_Code=M11Supp_Code inner join view_category on M09Cat_Code=M11Cat_Code where M11Status='A' and M07Sup_Name='" & Trim(cboSupplier_1.Text) & "' order by M11id"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 40
            UltraGrid1.Rows.Band.Columns(1).Width = 90
            UltraGrid1.Rows.Band.Columns(2).Width = 120
            UltraGrid1.Rows.Band.Columns(3).Width = 120
            UltraGrid1.Rows.Band.Columns(4).Width = 260
            UltraGrid1.Rows.Band.Columns(5).Width = 120
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_1()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11Part_No) ##,M11Part_No as [Item Code],M07Sup_Name as [Supplier Name],M09Cat_Name as [Category],M11Part_Name as [Item Name],CAST(M11Retail_price AS DECIMAL(16,2)) as Rate from M11Product_Item inner join M07Supplier on M07Sup_Code=M11Supp_Code inner join view_category on M09Cat_Code=M11Cat_Code where M11Status='I' order by M11id"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 40
            UltraGrid1.Rows.Band.Columns(1).Width = 90
            UltraGrid1.Rows.Band.Columns(2).Width = 120
            UltraGrid1.Rows.Band.Columns(3).Width = 120
            UltraGrid1.Rows.Band.Columns(4).Width = 260
            UltraGrid1.Rows.Band.Columns(5).Width = 120
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_Category()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M11Part_No) ##,M11Part_No as [Item Code],M07Sup_Name as [Supplier Name],M09Cat_Name as [Category],M11Part_Name as [Item Name],CAST(M11Retail_price AS DECIMAL(16,2)) as Rate from M11Product_Item inner join M07Supplier on M07Sup_Code=M11Supp_Code inner join view_category on M09Cat_Code=M11Cat_Code where M11Status='A' and M09Cat_Name='" & Trim(cboCategory_1.Text) & "' order by M11id"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 40
            UltraGrid1.Rows.Band.Columns(1).Width = 90
            UltraGrid1.Rows.Band.Columns(2).Width = 120
            UltraGrid1.Rows.Band.Columns(3).Width = 120
            UltraGrid1.Rows.Band.Columns(4).Width = 260
            UltraGrid1.Rows.Band.Columns(5).Width = 120
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            UltraGrid1.Rows.Band.Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Update_Image()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim IP As String
        Dim _STName As String
        Dim _PIC_Path As String
        Dim connection As New SqlConnection(ConfigurationManager.AppSettings("CD"))
        '  Dim command As New SqlCommand("insert into M31Vehicle_Master(M31Vehicle_No,M31BRAND,M31Pic,m31Engin_No,M31Chasis_no,M31Fuel,M31Type,M31Next_Lis,M31Next_Insu,M31Pic_Path,M31Status,M31Capacity) values(@name,@desc,@img,@ENG_NO,@M31Chasis_no,@M31Fuel,@M31Type,@M31Next_Lis,@M31Next_Insu,@M31Pic_Path,@M31Status,@M31Capacity)", connection)

        Try

            'MsgBox(Trim(txtEntry.Text))
            IP = ""
            Sql = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(txtCode.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)

            If isValidDataset(M01) Then

                Dim ms As New MemoryStream
                Dim ms1 As New MemoryStream
                '  ms.Dispose()
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                PictureBox2.Image.Save(ms1, PictureBox2.Image.RawFormat)
                Dim command As New SqlCommand("UPDATE M11Product_Item SET M11Img1=@Img,M11Img2=@Img1 WHERE  M11Part_No='" & Trim(txtCode.Text) & "'", connection)
                command.Parameters.Add("@img", SqlDbType.Image).Value = ms.ToArray()
                command.Parameters.Add("@img1", SqlDbType.Image).Value = ms1.ToArray()
                connection.Open()
                If command.ExecuteNonQuery() = 1 Then
                    ' MsgBox("test1", MsgBoxStyle.Information, "Information .......")
                    '  MsgBox("Records update Successfully", MsgBoxStyle.Information, "Information .......")

                Else

                    '  MsgBox("test", MsgBoxStyle.Information, "Information .......")

                End If
                connection.Close()
                ms.Dispose()

            End If
            con.CLOSE()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Private Sub cmdpic_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdpic_1.Click
        On Error Resume Next
        OpenFileDialog1.Filter = "Image Files|*.jpg;*.gif;*.png;*.bmp"
        OpenFileDialog1.ShowDialog()
        PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
        txtPic1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub cmdPic_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPic_2.Click
        On Error Resume Next
        OpenFileDialog1.Filter = "Image Files|*.jpg;*.gif;*.png;*.bmp"
        OpenFileDialog1.ShowDialog()
        PictureBox2.Image = Image.FromFile(OpenFileDialog1.FileName)
        txtPic2.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        Call Load_Grid_1()
        OPR0.Visible = False
        Panel2.Visible = False
        Panel1.Visible = False
        Panel5.Visible = False
        Call Clear_Text()
    End Sub

    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        On Error Resume Next
        Dim _Row As Integer
        _Row = UltraGrid1.ActiveRow.Index
        txtCode.Text = Trim(UltraGrid1.Rows(_Row).Cells(1).Text)
        Call Search_Records()
        OPR0.Visible = True
    End Sub

    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double

        Try
            Sql = "select  *  from View_Product_Items where M11Part_No='" & Trim(txtCode.Text) & "' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                cboSupplier.Text = Trim(M01.Tables(0).Rows(0)("M07Sup_Name"))
                txtType.Text = Trim(M01.Tables(0).Rows(0)("M11Type"))
                Call Load_Category()
                cboCategory.Text = Trim(M01.Tables(0).Rows(0)("M09Cat_Name"))
                txtName.Text = Trim(M01.Tables(0).Rows(0)("M11Part_Name"))
                txtRemark.Text = Trim(M01.Tables(0).Rows(0)("M11Remark"))
                Value = Trim(M01.Tables(0).Rows(0)("M11Re_Order"))
                txtReorder.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtReorder.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                Value = Trim(M01.Tables(0).Rows(0)("M11Retail_price"))
                txtRate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                txtPic1.Text = "-"
                txtPic2.Text = "-"
                Dim arrayImage() As Byte = CType(M01.Tables(0).Rows(0)("M11Img1"), Byte())
                Dim ms As New MemoryStream(arrayImage)
                PictureBox1.Image = Image.FromStream(ms)
                Dim arrayImage1() As Byte = CType(M01.Tables(0).Rows(0)("M11Img2"), Byte())
                Dim ms1 As New MemoryStream(arrayImage)
                PictureBox2.Image = Image.FromStream(ms1)
            End If


            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Private Sub ByCategoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ByCategoryToolStripMenuItem.Click
        Call Find_CATEGORY()
        cboCategory_1.Text = ""
        OPR0.Visible = False
        Panel2.Visible = True
        Panel1.Visible = False
        Panel5.Visible = False
        cboCategory_1.ToggleDropdown()
    End Sub

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        Panel2.Visible = False
        OPR0.Visible = False
        Call Load_Grid_Category()
    End Sub

    Private Sub BySupplierToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BySupplierToolStripMenuItem.Click
        Call Load_Supplier()
        cboSupplier_1.Text = ""
        OPR0.Visible = False
        Panel2.Visible = False
        Panel1.Visible = True
        cboSupplier_1.ToggleDropdown()
    End Sub

    Private Sub UltraButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton3.Click
        Call Load_Grid_supplier()
        Panel1.Visible = False
    End Sub

    Private Sub ByItemNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ByItemNameToolStripMenuItem.Click
        Panel1.Visible = False
        Panel2.Visible = False
        Panel5.Visible = True
        OPR0.Visible = False
        txtName_1.Focus()
    End Sub

    Private Sub txtName_1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName_1.KeyUp
        If e.KeyCode = Keys.Escape Then
            Panel5.Visible = False
        End If
    End Sub

    Private Sub txtName_1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName_1.TextChanged
        Call Load_Grid_item()
    End Sub

    Private Sub UltraButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton4.Click
        Panel5.Visible = False

    End Sub

    Private Sub UltraButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton2.Click
        Dim A As String

        A = MsgBox("Are you sure you want to deactivate this Items", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Information ........")
        If A = vbYes Then
            Call Save_Data()
        End If
    End Sub

    Private Sub UltraButton30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton30.Click
        Call Clear_Text()
        OPR0.Visible = False
    End Sub
End Class