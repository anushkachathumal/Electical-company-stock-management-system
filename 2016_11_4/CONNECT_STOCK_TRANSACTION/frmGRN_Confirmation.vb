Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Public Class frmGRN_Confirmation
    Inherits System.Windows.Forms.Form
    Dim dsUser As DataSet
    Dim Clicked As String
    Dim c_dataCustomer1 As DataTable
    Dim _locCode As String

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub frmGRN_Confirmation_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        frmview_Confirmation.Close()
    End Sub

    Private Sub frmGRN_Confirmation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtDate.Text = Today
        txtSupplier.ReadOnly = True
        txtTo.ReadOnly = True
        txtRate.ReadOnly = True
        txtRate.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtQty.ReadOnly = True
        txtQty.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtRef.ReadOnly = True
        txtRef.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtCom_Invoice.ReadOnly = True
        txtTotal.ReadOnly = True
        txtTotal.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtCount.ReadOnly = True
        txtCount.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtNet.ReadOnly = True
        txtNet.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtFree.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtQty.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtConfirm.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        Call Load_EntryNo()
        Call Load_GRN()
        Call Load_Gride2()
        Me.UltraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
    End Sub
   
    Function Load_EntryNo()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='CNG'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtRef.Text = "CONF-00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtRef.Text = "CONF-0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtRef.Text = "CONF-" & M01.Tables(0).Rows(0)("P01No")
                End If
            End If

            'Sql = "select * from M04Location"
            'M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            'If isValidDataset(M01) Then
            '    cboTo.Text = M01.Tables(0).Rows(0)("M04Loc_Name")
            'End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Function Load_GRN()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select T01GRN_No as [##],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name] from M07Supplier inner join T01GRN_Header on T01Supp_Code=M07Sup_Code where T01Status='A' order by T01ID "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboEntry
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 108
                .Rows.Band.Columns(1).Width = 120
                .Rows.Band.Columns(2).Width = 220

            End With
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Function Load_Gride2()
        Dim CustomerDataClass As New DAL_InterLocation()
        c_dataCustomer1 = CustomerDataClass.MakeDataTableGRN_Conf
        UltraGrid1.DataSource = c_dataCustomer1
        With UltraGrid1
            .DisplayLayout.Bands(0).Columns(0).Width = 90
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False
            .DisplayLayout.Bands(0).Columns(1).Width = 210
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False

            .DisplayLayout.Bands(0).Columns(2).Width = 70
            .DisplayLayout.Bands(0).Columns(3).Width = 70
            .DisplayLayout.Bands(0).Columns(4).Width = 70
            .DisplayLayout.Bands(0).Columns(5).Width = 70
            .DisplayLayout.Bands(0).Columns(6).Width = 90
            ' .DisplayLayout.Bands(0).Columns(7).Width = 90
            ' .DisplayLayout.Bands(0).Columns(8).Width = 90

            .DisplayLayout.Bands(0).Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(4).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            .DisplayLayout.Bands(0).Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            ' .DisplayLayout.Bands(0).Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(1).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(2).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(3).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(4).CellActivation = Activation.NoEdit
            '  .DisplayLayout.Bands(0).Columns(5).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(6).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(7).CellActivation = Activation.NoEdit


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            '  .DisplayLayout.Bands(0).Columns(6).Width = 90
            ' .DisplayLayout.Bands(0).Columns(7).Width = 90

            ' .DisplayLayout.Bands(0).Columns(3).Width = 300
            '.DisplayLayout.Bands(0).Columns(4).Width = 300
        End With
    End Function

    Function Search_Records_CONFIRM()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer


        Try
            Sql = "select * from T01GRN_Header inner join M07Supplier on M07Sup_Code=T01Supp_Code inner join M03New_Location on M03Loc_Code=T01To_Loc inner join T21GRN_Confirm_Header on T21GRN=T01GRN_No  where  T01Status='CONFIRM' AND T21Ref_No='" & Trim(txtRef.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                txtCom_Invoice.Text = Trim(M01.Tables(0).Rows(0)("T01Com_Invoice"))
                ' txtDate.Text = Trim(M01.Tables(0).Rows(0)("T01Date"))
                cboEntry.Text = Trim(M01.Tables(0).Rows(0)("T01GRN_No"))
                txtSupplier.Text = Trim(M01.Tables(0).Rows(0)("M07Sup_Name"))
                _locCode = Trim(M01.Tables(0).Rows(0)("T01To_Loc"))
                txtTo.Text = Trim(M01.Tables(0).Rows(0)("M03Loc_Name"))
                'vALUE = Trim(M01.Tables(0).Rows(0)("T01Total_Discount"))
                'txtDiscount.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                'txtDiscount.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
            End If
            Call Load_Gride2()
            Sql = "SELECT T02Item_Code,max(M11Part_Name) as M11Part_Name,max(T02Cost) as T02Cost,sum(T02Qty) as T02Qty,sum(T02Free_Issue) as T02Free_Issue,sum(T02Con_Qty) as T02Con_Qty FROM T02GRN_Flutter INNER JOIN M11Product_Item ON M11Part_No=T02Item_Code WHERE T02GRN_No='" & Trim(cboEntry.Text) & "' AND T02Status='CONFIRM' group by T02Item_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            i = 0
            For Each DTRow2 As DataRow In M01.Tables(0).Rows

                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(M01.Tables(0).Rows(i)("T02Item_Code"))
                newRow("Item Name") = Trim(M01.Tables(0).Rows(i)("M11Part_Name"))
                vALUE = Trim(M01.Tables(0).Rows(i)("T02Cost"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Cost Price") = _st
                newRow("Qty") = Trim(M01.Tables(0).Rows(i)("T02Qty"))
                newRow("Free Issue") = Trim(M01.Tables(0).Rows(i)("T02Free_Issue"))
                newRow("Conf Qty") = Trim(M01.Tables(0).Rows(i)("T02Con_Qty"))
                vALUE = CDbl(M01.Tables(0).Rows(i)("T02Qty")) * CDbl(M01.Tables(0).Rows(i)("T02Cost"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Total") = _st

                c_dataCustomer1.Rows.Add(newRow)


                i = i + 1
            Next
            txtCount.Text = UltraGrid1.Rows.Count
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
            Call Calculation_Net()

            Sql = "select T02Item_Code as [##],MAX(M11Part_Name) AS [Item Name] from T02GRN_Flutter inner join M11Product_Item on T02Item_Code=M11Part_No where T02Status='A' AND T02GRN_No='" & Trim(cboEntry.Text) & "' GROUP BY T02Item_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCode
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 212
                .Rows.Band.Columns(1).Width = 320
                ' .Rows.Band.Columns(2).Width = 220

            End With
            con.CLOSE()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.CLOSE()
            End If
        End Try


    End Function

    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer


        Try
            Sql = "select * from T01GRN_Header inner join M07Supplier on M07Sup_Code=T01Supp_Code inner join M03New_Location on M03Loc_Code=T01To_Loc where  T01Status='A' AND T01GRN_No='" & Trim(cboEntry.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                txtCom_Invoice.Text = Trim(M01.Tables(0).Rows(0)("T01Com_Invoice"))
                ' txtDate.Text = Trim(M01.Tables(0).Rows(0)("T01Date"))
                ' txtPO.Text = Trim(M01.Tables(0).Rows(0)("T01Shipping_No"))
                txtSupplier.Text = Trim(M01.Tables(0).Rows(0)("M07Sup_Name"))
                _locCode = Trim(M01.Tables(0).Rows(0)("T01To_Loc"))
                txtTo.Text = Trim(M01.Tables(0).Rows(0)("M03Loc_Name"))
                'vALUE = Trim(M01.Tables(0).Rows(0)("T01Total_Discount"))
                'txtDiscount.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                'txtDiscount.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
            End If
            Call Load_Gride2()
            Sql = "SELECT T02Item_Code,max(M11Part_Name) as M11Part_Name,max(T02Cost) as T02Cost,sum(T02Qty) as T02Qty,sum(T02Free_Issue) as T02Free_Issue,sum(T02Con_Qty) as T02Con_Qty FROM T02GRN_Flutter INNER JOIN M11Product_Item ON M11Part_No=T02Item_Code WHERE T02GRN_No='" & Trim(cboEntry.Text) & "' AND T02Status='A' group by T02Item_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            i = 0
            For Each DTRow2 As DataRow In M01.Tables(0).Rows

                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(M01.Tables(0).Rows(i)("T02Item_Code"))
                newRow("Item Name") = Trim(M01.Tables(0).Rows(i)("M11Part_Name"))
                vALUE = Trim(M01.Tables(0).Rows(i)("T02Cost"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Cost Price") = _st
                newRow("Qty") = Trim(M01.Tables(0).Rows(i)("T02Qty"))
                newRow("Free Issue") = Trim(M01.Tables(0).Rows(i)("T02Free_Issue"))
                newRow("Conf Qty") = Trim(M01.Tables(0).Rows(i)("T02Con_Qty"))
                vALUE = CDbl(M01.Tables(0).Rows(i)("T02Qty")) * CDbl(M01.Tables(0).Rows(i)("T02Cost"))
                _st = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _st = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Total") = _st

                c_dataCustomer1.Rows.Add(newRow)


                i = i + 1
            Next
            txtCount.Text = UltraGrid1.Rows.Count

            Call Calculation_Net()

            Sql = "select T02Item_Code as [##],MAX(M11Part_Name) AS [Item Name] from T02GRN_Flutter inner join M11Product_Item on T02Item_Code=M11Part_No where T02Status IN ('A','CONFIRM') AND T02GRN_No='" & Trim(cboEntry.Text) & "' GROUP BY T02Item_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCode
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 212
                .Rows.Band.Columns(1).Width = 320
                ' .Rows.Band.Columns(2).Width = 220

            End With
            con.CLOSE()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.CLOSE()
            End If
        End Try


    End Function
    Function Calculation_Net()
        On Error Resume Next
        Dim i As Integer
        Dim Value As Double

        For Each uRow As UltraGridRow In UltraGrid1.Rows
            ' MsgBox(Double.TryParse(txtNett.Text, value))
            Value = Value + CDbl((UltraGrid1.Rows(i).Cells(6).Value))
            i = i + 1
        Next
        txtNet.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtNet.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
       
    End Function

    Function Clear_tex()
        Me.cboEntry.Text = ""
        Me.cboCode.Text = ""
        Me.cboItemName.Text = ""
        Me.txtRemark.Text = ""
        Me.txtRate.Text = ""
        Me.txtQty.Text = ""
        Me.txtFree.Text = ""
        Me.txtTotal.Text = ""
        Me.txtConfirm.Text = ""
        Me.txtCom_Invoice.Text = ""
        Me.txtSupplier.Text = ""
        Me.txtTotal.Text = ""
        Me.txtTo.Text = ""
        Me.txtNet.Text = ""
        Call Load_Gride2()
        Me.txtCount.Text = ""
        Call Load_EntryNo()
    End Function
    Private Sub cboEntry_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEntry.AfterCloseUp
        Call Search_Records()
    End Sub

    Private Sub cboEntry_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboEntry.KeyUp
        If e.KeyCode = 13 Then
            cboCode.ToggleDropdown()
        End If
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call Clear_tex()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If txtRemark.Text <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        If UltraGrid1.Rows.Count > 0 Then
        Else
            MsgBox("Please enter the Records", MsgBoxStyle.Information, "Information ......")
            Exit Sub
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
        Dim i As Integer
        Dim _GETDATE As DateTime
        Dim _GET_TIME As DateTime
        Dim M01 As DataSet

        Try
            'nvcFieldList1 = "SELECT * FROM T21GRN_Confirm_Header where T21Ref_No='" & Trim(txtRef.Text) & "' and T01Cost_Status='NO'"
            'MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            'If isValidDataset(MB51) Then
            '    MsgBox("Can't Confirm ")
            'End If
            '===================================================================================================
            nvcFieldList1 = "SELECT * FROM T21GRN_Confirm_Header where T21Ref_No='" & Trim(txtRef.Text) & "'"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                MsgBox("This GRN Confirmation alrady exsist", MsgBoxStyle.Information, "Information ........")
                connection.Close()
                Exit Function
            Else
                _GETDATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)

                _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

                Call Load_EntryNo()

                nvcFieldList1 = "update T01GRN_Header set T01Status='CONFIRM' where T01GRN_No='" & Trim(cboEntry.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='CNG' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into T21GRN_Confirm_Header(T21Ref_No,T21Date,T21GRN,T21Com_Invoice,T21Status)" & _
                                                                  " values('" & Trim(txtRef.Text) & "', '" & _GETDATE & "','" & Trim(cboEntry.Text) & "','" & txtCom_Invoice.Text & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                              " values('GRN_CONF', 'SAVE','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtRef.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)


                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    If IsNumeric(Trim(UltraGrid1.Rows(i).Cells(5).Text)) Then

                        nvcFieldList1 = "Insert Into T22GRN_Confirm_Flutter(T22Ref_No,T22Item_Code,T22Cost,T22Qty,T22Free,T22Con_Qty,T22Status)" & _
                                                                          " values('" & Trim(txtRef.Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(2).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(3).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(4).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(5).Text) & "','A')"
                        ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                        nvcFieldList1 = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "'"
                        M01 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                        If isValidDataset(M01) Then
                            If Trim(M01.Tables(0).Rows(0)("M11Type")) = "PRODUCT ITEM" Then
                                nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                                               " values('" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "', '" & _locCode & "','" & _GETDATE & "','GRN','" & Trim(txtRef.Text) & "','" & CInt(UltraGrid1.Rows(i).Cells(4).Text) + CInt(UltraGrid1.Rows(i).Cells(5).Text) & "','A')"
                                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                            ElseIf Trim(M01.Tables(0).Rows(0)("M11Type")) = "ROW ITEMS" Then
                                nvcFieldList1 = "Insert Into S02Row_Stock(S02Part_no,S02Loc_Code,S02Date,S02Tr_Type,S02Ref_No,S02Qty,S02Status)" & _
                                                                             " values('" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "', '" & _locCode & "','" & _GETDATE & "','GRN','" & Trim(txtRef.Text) & "','" & CInt(UltraGrid1.Rows(i).Cells(4).Text) + CInt(UltraGrid1.Rows(i).Cells(5).Text) & "','A')"
                                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                            End If
                        End If
                        '=========================================================================================================

                        nvcFieldList1 = "UPDATE T02GRN_Flutter SET T02Con_Qty='" & CInt(UltraGrid1.Rows(i).Cells(4).Text) + CInt(UltraGrid1.Rows(i).Cells(5).Text) & "',T02Status='CONFIRM' WHERE T02GRN_No='" & Trim(cboEntry.Text) & "' AND T02Item_Code='" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "'"
                        ExecuteNonQueryText(connection, transaction, nvcFieldList1)



                    End If
                    i = i + 1
                Next
                MsgBox("Records update successfully", MsgBoxStyle.Information, "Information .......")
                transaction.Commit()
                connection.Close()
                Call Clear_tex()
                ' Call Load_EntryNo()
                Call Load_GRN()
            End If
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function
    Function Search_Item() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer
        Try
            Sql = "SELECT T02Item_Code,max(M11Part_Name) as M11Part_Name,max(T02Cost) as T02Cost,sum(T02Qty) as T02Qty,sum(T02Free_Issue) as T02Free_Issue,sum(T02Con_Qty) as T02Con_Qty FROM T02GRN_Flutter INNER JOIN M11Product_Item ON M11Part_No=T02Item_Code WHERE T02GRN_No='" & Trim(cboEntry.Text) & "' AND T02Status='A'  and T02Item_Code='" & Trim(cboCode.Text) & "' group by T02Item_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Item = True
                cboItemName.Text = Trim(M01.Tables(0).Rows(0)("M11Part_Name"))
                vALUE = Trim(M01.Tables(0).Rows(0)("T02Cost"))
                txtRate.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                txtQty.Text = Trim(M01.Tables(0).Rows(0)("T02Qty"))
                txtFree.Text = Trim(M01.Tables(0).Rows(0)("T02Free_Issue"))
                vALUE = CDbl(M01.Tables(0).Rows(0)("T02Cost")) * CDbl(M01.Tables(0).Rows(0)("T02Qty"))
                txtTotal.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtTotal.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
            End If
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.CLOSE()
            End If
        End Try

    End Function

    Function Search_Item_name() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim vALUE As Double
        Dim _st As String
        Dim i As Integer
        Try
            Sql = "SELECT T02Item_Code,max(M11Part_Name) as M11Part_Name,max(T02Cost) as T02Cost,sum(T02Qty) as T02Qty,sum(T02Free_Issue) as T02Free_Issue,sum(T02Con_Qty) as T02Con_Qty FROM T02GRN_Flutter INNER JOIN M11Product_Item ON M11Part_No=T02Item_Code WHERE T02GRN_No='" & Trim(cboEntry.Text) & "' AND T02Status='A'  and M11Part_Name='" & Trim(cboItemName.Text) & "' group by T02Item_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Item_name = True
                cboCode.Text = Trim(M01.Tables(0).Rows(0)("T02Item_Code"))
                vALUE = Trim(M01.Tables(0).Rows(0)("T02Cost"))
                txtRate.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                txtQty.Text = Trim(M01.Tables(0).Rows(0)("T02Qty"))
                txtFree.Text = Trim(M01.Tables(0).Rows(0)("T02Free_Issue"))
                vALUE = CDbl(M01.Tables(0).Rows(0)("T02Cost")) * CDbl(M01.Tables(0).Rows(0)("T02Qty"))
                txtTotal.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtTotal.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
            End If
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.CLOSE()
            End If
        End Try

    End Function

    Private Sub cboCode_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCode.AfterCloseUp
        Call Search_Item()
    End Sub

    Private Sub cboCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCode.KeyUp
        If e.KeyCode = 13 Then
            Call Search_Item()
            txtConfirm.Focus()
        End If
    End Sub

    Private Sub txtConfirm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtConfirm.KeyUp
        Dim i As Integer
        Try
            If e.KeyCode = 13 Then
                If Search_Item() = True Then
                Else
                    MsgBox("Please enter the correct Item", MsgBoxStyle.Information, "Information ........")
                    txtConfirm.Focus()
                    Exit Sub
                End If

                If txtConfirm.Text <> "" Then
                Else
                    MsgBox("Please enter the confirm qty", MsgBoxStyle.Information, "Information .......")
                    txtConfirm.Focus()
                    Exit Sub
                End If
                If IsNumeric(txtConfirm.Text) Then
                Else
                    MsgBox("Please enter the correct Item", MsgBoxStyle.Information, "Information ......")
                    txtConfirm.Focus()
                    Exit Sub
                End If
                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    If Trim(cboCode.Text) = Trim(UltraGrid1.Rows(i).Cells(0).Text) Then
                        UltraGrid1.Rows(i).Cells(5).Value = Trim(txtConfirm.Text)
                        Exit For
                    End If
                    i = i + 1
                Next
                txtConfirm.Text = ""
                txtRate.Text = ""
                txtTotal.Text = ""
                txtQty.Text = ""
                cboCode.Text = ""
                txtFree.Text = ""
                cboItemName.Text = ""
                cboCode.ToggleDropdown()
            End If
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                ' con.CLOSE()
            End If
        End Try
    End Sub

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        frmview_Confirmation.Close()
        frmview_Confirmation.Show()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If txtRemark.Text <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        If UltraGrid1.Rows.Count > 0 Then
        Else
            MsgBox("Please enter the Records", MsgBoxStyle.Information, "Information ......")
            Exit Sub
        End If

        Call EDIT_DATA()
    End Sub

    Function EDIT_DATA()
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
        Dim i As Integer
        Dim _GETDATE As DateTime
        Dim _GET_TIME As DateTime
        Dim M01 As DataSet

        Try
            nvcFieldList1 = "SELECT * FROM T21GRN_Confirm_Header where T21Ref_No='" & Trim(txtRef.Text) & "'"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then

                If Trim(MB51.Tables(0).Rows(0)("T21Status")) = "CANCEL" Then
                    MsgBox("This record alrady cancel", MsgBoxStyle.Information, "Information .......")
                    connection.Close()
                    Exit Function
                End If
                _GETDATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)

                _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

                '


                nvcFieldList1 = "UPDATE T21GRN_Confirm_Header SET T21Date='" & _GETDATE & "',T21Status='A' WHERE T21Ref_No='" & Trim(txtRef.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                'nvcFieldList1 = "Insert Into T21GRN_Confirm_Header(T21Ref_No,T21Date,T21GRN,T21Com_Invoice,T21Status)" & _
                '                                                  " values('" & Trim(txtRef.Text) & "', '" & _GETDATE & "','" & Trim(cboEntry.Text) & "','" & txtCom_Invoice.Text & "','A')"
                'ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                              " values('GRN_CONF', 'EDIT','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtRef.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "DELETE FROM T22GRN_Confirm_Flutter WHERE T22Ref_No='" & Trim(txtRef.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "DELETE FROM S01Stock_Product_Items WHERE S01Ref_No='" & Trim(txtRef.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)


                nvcFieldList1 = "DELETE FROM S02Row_Stock WHERE S02Ref_No='" & Trim(txtRef.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    If IsNumeric(Trim(UltraGrid1.Rows(i).Cells(5).Text)) Then

                        nvcFieldList1 = "Insert Into T22GRN_Confirm_Flutter(T22Ref_No,T22Item_Code,T22Cost,T22Qty,T22Free,T22Con_Qty,T22Status)" & _
                                                                          " values('" & Trim(txtRef.Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(2).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(3).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(4).Text) & "','" & Trim(UltraGrid1.Rows(i).Cells(5).Text) & "','A')"
                        ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                        nvcFieldList1 = "SELECT * FROM M11Product_Item WHERE M11Part_No='" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "'"
                        M01 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                        If isValidDataset(M01) Then
                            If Trim(M01.Tables(0).Rows(0)("M11Type")) = "PRODUCT ITEM" Then
                                nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                                               " values('" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "', '" & _locCode & "','" & _GETDATE & "','GRN','" & Trim(txtRef.Text) & "','" & CInt(UltraGrid1.Rows(i).Cells(4).Text) + CInt(UltraGrid1.Rows(i).Cells(5).Text) & "','A')"
                                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                            ElseIf Trim(M01.Tables(0).Rows(0)("M11Type")) = "ROW ITEMS" Then
                                nvcFieldList1 = "Insert Into S02Row_Stock(S02Part_no,S02Loc_Code,S02Date,S02Tr_Type,S02Ref_No,S02Qty,S02Status)" & _
                                                                             " values('" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "', '" & _locCode & "','" & _GETDATE & "','GRN','" & Trim(txtRef.Text) & "','" & CInt(UltraGrid1.Rows(i).Cells(4).Text) + CInt(UltraGrid1.Rows(i).Cells(5).Text) & "','A')"
                                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                            End If
                        End If
                        '=========================================================================================================

                        nvcFieldList1 = "UPDATE T02GRN_Flutter SET T02Con_Qty='" & CInt(UltraGrid1.Rows(i).Cells(4).Text) + CInt(UltraGrid1.Rows(i).Cells(5).Text) & "',T02Status='CONFIRM' WHERE T02GRN_No='" & Trim(cboEntry.Text) & "' AND T02Item_Code='" & Trim(UltraGrid1.Rows(i).Cells(0).Text) & "'"
                        ExecuteNonQueryText(connection, transaction, nvcFieldList1)



                    End If
                    i = i + 1
                Next
                MsgBox("Records update successfully", MsgBoxStyle.Information, "Information .......")
                transaction.Commit()
                connection.Close()
                Call Clear_tex()
                ' Call Load_EntryNo()
                Call Load_GRN()
            End If
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function

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
        Dim i As Integer
        Dim _GETDATE As DateTime
        Dim _GET_TIME As DateTime
        Dim M01 As DataSet
        Dim A As String
        Try
            _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            A = MsgBox("Are you sure you want to delete this records", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Delete .........")
            If A = vbYes Then
                nvcFieldList1 = "UPDATE T21GRN_Confirm_Header SET T21Status='CANCEL' WHERE T21Ref_No='" & Trim(txtRef.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T22GRN_Confirm_Flutter SET T22Status='CANCEL' WHERE T22Ref_No='" & Trim(txtRef.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE S01Stock_Product_Items SET S01Status='CANCEL' WHERE S01Ref_No='" & Trim(txtRef.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE S02Row_Stock SET S02Status='CANCEL' WHERE S02Ref_No='" & Trim(txtRef.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T01GRN_Header SET T01Status='A' WHERE T01GRN_No='" & Trim(cboEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T02GRN_Flutter SET T02Status='A',T02Con_Qty='0' WHERE T02GRN_No='" & Trim(cboEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                          " values('GRN_CONF', 'DELETE','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtRef.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                MsgBox("Transaction deleted successfully", MsgBoxStyle.Information, "Information ........")
                transaction.Commit()
                connection.Close()
            End If
            Call Clear_tex()
            cmdDelete.Enabled = True
            cmdEdit.Enabled = True
            Call Load_GRN()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Sub

    Private Sub cboItemName_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboItemName.AfterCloseUp
        Call Search_Item_name()
    End Sub

    
    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Call Clear_tex()
    End Sub
End Class