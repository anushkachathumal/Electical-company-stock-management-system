﻿Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Public Class frmSales_Ref_Cnt
    Inherits System.Windows.Forms.Form
    Dim dsUser As DataSet
    Dim Clicked As String
    Dim c_dataCustomer1 As DataTable
    Dim _CountryCode As String
    Dim _Comcode As String
    Private Sub frmSales_Ref_Cnt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCode.ReadOnly = True
        txtCode.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        Call Load_Grid()
        Call Load_Entry()
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Function Clear_Text()
        Me.txtName.Text = ""
        Me.txtNIC.Text = ""
        Me.txtTp.Text = ""
        txtName.Focus()
    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call Clear_Text()
    End Sub

    Private Sub txtName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyUp
        If e.KeyCode = 13 Then
            If Trim(txtName.Text) <> "" Then
                txtTp.Focus()
            End If
        End If
    End Sub

    Private Sub txtTp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTp.KeyUp
        If e.KeyCode = 13 Then
            txtNIC.Focus()
        End If
    End Sub

    Private Sub txtNIC_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNIC.KeyUp
        If e.KeyCode = 13 Then
            cmdAdd.Focus()
        End If
    End Sub


    
    Function Load_Entry()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='SR' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtCode.Text = "SR/00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtCode.Text = "SR/0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtCode.Text = "SR/" & M01.Tables(0).Rows(0)("P01No")
                End If
            End If


            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If Trim(txtName.Text) <> "" Then
        Else
            MsgBox("Please enter the Sales Ref Name", MsgBoxStyle.Information, "Information ........")
            Exit Sub
        End If

        If Trim(txtTp.Text) <> "" Then
        Else
            txtTp.Text = "-"
        End If

        If Trim(txtNIC.Text) <> "" Then
        Else
            txtNIC.Text = "-"
        End If

        Call Save_Data()
    End Sub

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
        Try

            nvcFieldList1 = "SELECT * FROM M04Create_Sale_Ref WHERE M04Rer_No='" & Trim(txtCode.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                'nvcFieldList1 = "update M04Create_Sale_Ref set M04Ref_Name='" & Trim(txtName.Text) & "',M04Contact_No='" & Trim(txtTp.Text) & "',M04ID_No='" & Trim(txtNIC.Text) & "' where M04Rer_No='" & Trim(txtCode.Text) & "' "
                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                'nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                '                                               " values('NEW_SALES_REF','EDIT', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)
            Else
                nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='SR' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into M04Create_Sale_Ref(M04Rer_No,M04Ref_Name,M04Contact_No,M04ID_No,M04Status)" & _
                                                                  " values('" & Trim(txtCode.Text) & "','" & Trim(txtName.Text) & "', '" & Trim(txtTp.Text) & "','" & Trim(txtNIC.Text) & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                " values('NEW_SALES_REF','SAVE', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            End If
            MsgBox("Record update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            txtName.Text = ""
            txtTp.Text = ""
            txtNIC.Text = ""
            txtName.Focus()
            Call Load_Grid()
            Call Load_Entry()
            connection.Close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                DBEngin.CloseConnection(connection)
                connection.ConnectionString = ""
            End If
        End Try
    End Function

    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from M04Create_Sale_Ref where  M04Rer_No='" & Trim(txtCode.Text) & "' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                txtName.Text = Trim(M01.Tables(0).Rows(0)("M04Ref_Name"))
                txtTp.Text = Trim(M01.Tables(0).Rows(0)("M04Contact_No"))
                txtNIC.Text = Trim(M01.Tables(0).Rows(0)("M04ID_No"))
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
            End If


            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Function EDIT_Data()
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

            nvcFieldList1 = "SELECT * FROM M04Create_Sale_Ref WHERE M04Rer_No='" & Trim(txtCode.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "update M04Create_Sale_Ref set M04Ref_Name='" & Trim(txtName.Text) & "',M04Contact_No='" & Trim(txtTp.Text) & "',M04ID_No='" & Trim(txtNIC.Text) & "' where M04Rer_No='" & Trim(txtCode.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                               " values('NEW_SALES_REF','EDIT', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
            Else
                'nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='SR' "
                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                'nvcFieldList1 = "Insert Into M04Create_Sale_Ref(M04Rer_No,M04Ref_Name,M04Contact_No,M04ID_No,M04Status)" & _
                '                                                  " values('" & Trim(txtCode.Text) & "','" & Trim(txtName.Text) & "', 'A','" & Trim(txtTp.Text) & "','" & Trim(txtNIC.Text) & "','A')"
                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                'nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                '                                                " values('NEW_SALES_REF','SAVE', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            End If
            MsgBox("Record update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            txtName.Text = ""
            txtTp.Text = ""
            txtNIC.Text = ""
            txtName.Focus()
            Call Load_Grid()
            Call Load_Entry()
            connection.Close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                DBEngin.CloseConnection(connection)
                connection.ConnectionString = ""
            End If
        End Try
    End Function

    Function Load_Grid()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M04Rer_No  ) ##,M04Rer_No as [Ref.Code],M04Ref_Name as [Ref Name],M04Contact_No  as [Contact No]  from M04Create_Sale_Ref where M04Status='A' order by M04ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 230
            UltraGrid1.Rows.Band.Columns(3).Width = 90
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_filter_Name()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  ROW_NUMBER() OVER (ORDER BY M04Rer_No  ) ##,M04Rer_No as [Ref.Code],M04Ref_Name as [Ref Name],M04Contact_No  as [Contact No]  from M04Create_Sale_Ref where M04Status='A' and M04Ref_Name like '%" & txtName.Text & "%' order by M04ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 30
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 230
            UltraGrid1.Rows.Band.Columns(3).Width = 90
            UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Private Sub txtName_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.ValueChanged
        Call Load_Grid_filter_Name()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If Trim(txtName.Text) <> "" Then
        Else
            MsgBox("Please enter the Sales Ref Name", MsgBoxStyle.Information, "Information ........")
            Exit Sub
        End If

        If Trim(txtTp.Text) <> "" Then
        Else
            txtTp.Text = "-"
        End If

        If Trim(txtNIC.Text) <> "" Then
        Else
            txtNIC.Text = "-"
        End If
        Call EDIT_Data()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
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
        Dim A As String

        Try
            A = MsgBox("Are you sure you want to Delete this records", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Cancel ....")
            If A = vbYes Then
                nvcFieldList1 = "SELECT * FROM M04Create_Sale_Ref WHERE M04Rer_No='" & Trim(txtCode.Text) & "' "
                MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                If isValidDataset(MB51) Then
                    nvcFieldList1 = "update M04Create_Sale_Ref set M04Status='I' where M04Rer_No='" & Trim(txtCode.Text) & "' "
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                    nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                   " values('NEW_SALES_REF','DELETE', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                    MsgBox("Root create successfully", MsgBoxStyle.Information, "Information ..........")
                Else

                    MsgBox("Record not available  for delete", MsgBoxStyle.Information, "Information .....")
                End If
            End If

            transaction.Commit()
            txtName.Text = ""
            txtNIC.Text = ""
            txtNIC.Text = ""
            txtName.Focus()
            Call Load_Grid()
            Call Load_Entry()
            connection.Close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                DBEngin.CloseConnection(connection)
                connection.ConnectionString = ""
            End If
        End Try
    End Sub

    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        On Error Resume Next
        Dim _Row As Integer

        _Row = UltraGrid1.ActiveRow.Index
        txtCode.Text = Trim(UltraGrid1.Rows(_Row).Cells(1).Text)
        Call Search_Records()
    End Sub
End Class