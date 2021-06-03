Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Public Class frmGRN_Budget
    Inherits System.Windows.Forms.Form
    Dim dsUser As DataSet
    Dim Clicked As String
    Dim c_dataCustomer1 As DataTable

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Function Load_EntryNo()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='COST'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtEntry.Text = "CST-00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtEntry.Text = "CST-0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtEntry.Text = "CST-" & M01.Tables(0).Rows(0)("P01No")
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


    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double
        Dim i As Integer
        Dim _TOTALQTY As Integer
        Dim _St As String
        Dim _Totalcost As Double
        Try
            Sql = "select * from M24GRN_Cost_Analysis_Header where M24Ref_No='" & Trim(txtEntry.Text) & "' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                txtDate.Text = M01.Tables(0).Rows(0)("M24Date")
                Value = M01.Tables(0).Rows(0)("M24USD_Rate")
                txtUSD.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtUSD.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                Value = M01.Tables(0).Rows(0)("M24Unit_Cost")
                txtU_Cost.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtU_Cost.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                Value = M01.Tables(0).Rows(0)("M24Tax")
                txtTax.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtTax.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                Value = M01.Tables(0).Rows(0)("M24Cus_Deuty")
                txtCustom.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtCustom.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                Value = M01.Tables(0).Rows(0)("M24Shp_Chargers")
                txtS_chargers.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtS_chargers.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))


                Value = M01.Tables(0).Rows(0)("M24Comm")
                txtComm.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtComm.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))


                Value = M01.Tables(0).Rows(0)("M24Transport")
                txtTransport.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtTransport.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                Value = M01.Tables(0).Rows(0)("M24Bank")
                txtBank.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtBank.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                Value = M01.Tables(0).Rows(0)("M24VAT")
                txtVat.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtVat.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                Value = M01.Tables(0).Rows(0)("M24NBT")
                txtNBT.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtNBT.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                cmdAdd.Enabled = True
                cmdDelete.Enabled = True

                Call CALCULATING_TOTAL()
                Call Load_Gride2()
                Sql = "SELECT T02Item_Code,max(M11Part_Name) as M11Part_Name,max(T02Cost) as T02Cost,sum(T02Qty) as T02Qty FROM T02GRN_Flutter INNER JOIN M11Product_Item ON M11Part_No=T02Item_Code WHERE T02GRN_No='" & Trim(cboGRN.Text) & "' AND T02Status IN ('CONFIRM','A') group by T02Item_Code,T02GRN_No "
                M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
                i = 0
                For Each DTRow2 As DataRow In M01.Tables(0).Rows
                    Dim newRow As DataRow = c_dataCustomer1.NewRow

                    newRow("Item Code") = Trim(M01.Tables(0).Rows(i)("T02Item_Code"))
                    newRow("Item Name") = Trim(M01.Tables(0).Rows(i)("M11Part_Name"))
                    Value = CDbl(M01.Tables(0).Rows(i)("T02Cost"))
                    _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                    newRow("Cost(USD)") = _St

                    Value = CDbl(M01.Tables(0).Rows(i)("T02Cost")) * CDbl(txtUSD.Text)
                    _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                    newRow("Cost Price") = _St
                    newRow("Qty") = Trim(M01.Tables(0).Rows(i)("T02Qty"))
                    newRow("#Costing Price") = "0"
                    Value = Value * CDbl(M01.Tables(0).Rows(i)("T02Qty"))
                    _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                    newRow("Total Cost") = _St
                    newRow("Total Amount") = "0"
                    c_dataCustomer1.Rows.Add(newRow)
                    i = i + 1
                Next
                i = 0
                Sql = "SELECT SUM(T02Qty) AS T02Qty FROM T02GRN_Flutter WHERE  T02GRN_No='" & Trim(cboGRN.Text) & "' AND T02Status IN ('CONFIRM','A') GROUP BY T02GRN_No"
                M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
                If isValidDataset(M01) Then
                    _TOTALQTY = M01.Tables(0).Rows(0)("T02Qty")
                End If

                _Totalcost = CDbl(txtTotal.Text) / _TOTALQTY
                'txtU_Cost.Text = (_Totalcost.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                'txtU_Cost.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", _Totalcost))

                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    If IsNumeric(UltraGrid1.Rows(i).Cells(4).Text) Then
                        Value = CDbl(UltraGrid1.Rows(i).Cells(4).Text) * CDbl(txtU_Cost.Text)
                        _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                        _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                        UltraGrid1.Rows(i).Cells(5).Value = _St
                        Value = CDbl(UltraGrid1.Rows(i).Cells(5).Text) + CDbl(UltraGrid1.Rows(i).Cells(6).Text)
                        _St = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                        _St = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                        UltraGrid1.Rows(i).Cells(7).Value = _St
                    End If
                    i = i + 1
                Next

            End If

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function
    Private Sub frmGRN_Budget_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        frmview_Cost.Close()
    End Sub
    Private Sub frmGRN_Budget_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtEntry.ReadOnly = True
        txtEntry.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtComm.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtVat.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtNBT.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtBank.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtS_chargers.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtTax.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtTotal.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtTotal.ReadOnly = True
        txtTransport.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtUSD.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDate.Text = Today
        txtCustom.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        Me.UltraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
        txtU_Cost.ReadOnly = True
        txtU_Cost.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        Call Load_EntryNo()
        Call Load_GRN()
        Call Load_Gride2()
    End Sub

    Function Load_GRN()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select T01GRN_No as [##],T01Com_Invoice as [Com.Inv.No],M07Sup_Name as [Supplier Name] from M07Supplier inner join T01GRN_Header on T01Supp_Code=M07Sup_Code where T01Status IN ('A','CONFIRM') and T01Cost_Status='NO' order by T01ID "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboGRN
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 142
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
        c_dataCustomer1 = CustomerDataClass.MakeDataTableCOST_GRN
        UltraGrid1.DataSource = c_dataCustomer1
        With UltraGrid1
            .DisplayLayout.Bands(0).Columns(0).Width = 90
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False
            .DisplayLayout.Bands(0).Columns(1).Width = 210
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False

            .DisplayLayout.Bands(0).Columns(2).Width = 80
            .DisplayLayout.Bands(0).Columns(3).Width = 90
            .DisplayLayout.Bands(0).Columns(4).Width = 80
            .DisplayLayout.Bands(0).Columns(5).Width = 90
            .DisplayLayout.Bands(0).Columns(6).Width = 110
            .DisplayLayout.Bands(0).Columns(7).Width = 110
            ' .DisplayLayout.Bands(0).Columns(8).Width = 90

            .DisplayLayout.Bands(0).Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(4).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            .DisplayLayout.Bands(0).Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
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

    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        If txtUSD.Text <> "" Then
        Else
            MsgBox("Please enter the $ rate", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If

        If IsNumeric(txtUSD.Text) Then
        Else
            MsgBox("Please enter correct USD Rate", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================== tax
        If txtTax.Text <> "" Then
        Else
            txtTax.Text = "0"
        End If

        If IsNumeric(txtTax.Text) Then
        Else
            MsgBox("Please enter correct Tax Payment", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '=============================================== Custom Duty

        If txtCustom.Text <> "" Then
        Else
            txtCustom.Text = "0"
        End If

        If IsNumeric(txtCustom.Text) Then
        Else
            MsgBox("Please enter correct Custom Duty", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '=============================================== Shipping Charges
        If txtS_chargers.Text <> "" Then
        Else
            txtS_chargers.Text = "0"
        End If

        If IsNumeric(txtS_chargers.Text) Then
        Else
            MsgBox("Please enter correct Custom Duty", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================== Commission
        If txtComm.Text <> "" Then
        Else
            txtComm.Text = "0"
        End If

        If IsNumeric(txtComm.Text) Then
        Else
            MsgBox("Please enter correct Commission", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================= Transport
        If txtTransport.Text <> "" Then
        Else
            txtTransport.Text = "0"
        End If

        If IsNumeric(txtTransport.Text) Then
        Else
            MsgBox("Please enter correct Transport Chargers", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================= Bank Chargers
        If txtBank.Text <> "" Then
        Else
            txtBank.Text = "0"
        End If

        If IsNumeric(txtBank.Text) Then
        Else
            MsgBox("Please enter correct Bank Chargers", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================ VAT
        If txtVat.Text <> "" Then
        Else
            txtVat.Text = "0"
        End If

        If IsNumeric(txtVat.Text) Then
        Else
            MsgBox("Please enter correct VAT", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '=========================================== NBT

        If txtNBT.Text <> "" Then
        Else
            txtNBT.Text = "0"
        End If

        If IsNumeric(txtNBT.Text) Then
        Else
            MsgBox("Please enter correct NBT", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If

        Call CALCULATING_TOTAL()
        Call lOAD_DATA_GRIDE()
    End Sub

    Function CALCULATING_TOTAL()
        On Error Resume Next
        Dim VALUE As Double

        VALUE = CDbl(txtTax.Text) + CDbl(txtBank.Text) + CDbl(txtComm.Text) + CDbl(txtCustom.Text) + CDbl(txtNBT.Text) + CDbl(txtTransport.Text) + CDbl(txtVat.Text) + CDbl(txtS_chargers.Text)
        txtTotal.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtTotal.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
    End Function
    Function lOAD_DATA_GRIDE() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim I As Integer
        Dim vALUE As Double
        Dim _ST As String
        Dim _TOTALQTY As Integer
        Dim _Totalcost As Double

        Try
            Call Load_Gride2()
            lOAD_DATA_GRIDE = False
            Sql = "SELECT T02Item_Code,max(M11Part_Name) as M11Part_Name,max(T02Cost) as T02Cost,sum(T02Qty) as T02Qty FROM T02GRN_Flutter INNER JOIN M11Product_Item ON M11Part_No=T02Item_Code WHERE T02GRN_No='" & Trim(cboGRN.Text) & "' AND T02Status IN ('CONFIRM','A') group by T02Item_Code,T02GRN_No "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            I = 0
            For Each DTRow2 As DataRow In M01.Tables(0).Rows
                Dim newRow As DataRow = c_dataCustomer1.NewRow
                lOAD_DATA_GRIDE = True
                newRow("Item Code") = Trim(M01.Tables(0).Rows(I)("T02Item_Code"))
                newRow("Item Name") = Trim(M01.Tables(0).Rows(I)("M11Part_Name"))
                vALUE = CDbl(M01.Tables(0).Rows(I)("T02Cost"))
                _ST = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Cost(USD)") = _ST

                vALUE = CDbl(M01.Tables(0).Rows(I)("T02Cost")) * CDbl(txtUSD.Text)
                _ST = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Cost Price") = _ST
                newRow("Qty") = Trim(M01.Tables(0).Rows(I)("T02Qty"))
                newRow("#Costing Price") = "0"
                vALUE = vALUE * CDbl(M01.Tables(0).Rows(I)("T02Qty"))
                _ST = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                newRow("Total Cost") = _ST
                newRow("Total Amount") = "0"
                c_dataCustomer1.Rows.Add(newRow)
                I = I + 1
            Next
            I = 0
            Sql = "SELECT SUM(T02Qty) AS T02Qty FROM T02GRN_Flutter WHERE  T02GRN_No='" & Trim(cboGRN.Text) & "' AND T02Status IN ('CONFIRM','A') GROUP BY T02GRN_No"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                _TOTALQTY = M01.Tables(0).Rows(0)("T02Qty")
            End If

            _Totalcost = CDbl(txtTotal.Text) / _TOTALQTY
            txtU_Cost.Text = (_Totalcost.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
            txtU_Cost.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", _Totalcost))

            For Each uRow As UltraGridRow In UltraGrid1.Rows
                If IsNumeric(UltraGrid1.Rows(I).Cells(4).Text) Then
                    vALUE = CDbl(UltraGrid1.Rows(I).Cells(4).Text) * CDbl(txtU_Cost.Text)
                    _ST = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                    UltraGrid1.Rows(I).Cells(5).Value = _ST
                    vALUE = CDbl(UltraGrid1.Rows(I).Cells(5).Text) + CDbl(UltraGrid1.Rows(I).Cells(6).Text)
                    _ST = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                    UltraGrid1.Rows(I).Cells(7).Value = _ST
                End If
                I = I + 1
            Next
            cmdAdd.Enabled = True

            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Function cLEAR_TEXT()
        Me.txtBank.Text = "0"
        Me.txtComm.Text = "0"
        Me.txtCustom.Text = "0"
        Me.txtTax.Text = "0"
        Me.txtTransport.Text = "0"
        Me.txtS_chargers.Text = "0"
        Me.txtVat.Text = "0"
        Me.txtNBT.Text = "0"
        Me.txtTotal.Text = ""
        Me.txtU_Cost.Text = ""
        Me.cmdAdd.Enabled = False
        cmdDelete.Enabled = False
        Call Load_Gride2()
    End Function

    Private Sub cboGRN_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGRN.AfterCloseUp
        Call cLEAR_TEXT()
    End Sub

    Private Sub txtTax_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTax.KeyUp
        Dim VALUE As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtTax.Text) Then
                VALUE = txtTax.Text
                txtTax.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtTax.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
            End If
            txtCustom.Focus()
        End If
    End Sub

    Private Sub txtCustom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCustom.KeyUp
        Dim VALUE As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtCustom.Text) Then
                VALUE = txtCustom.Text
                txtCustom.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtCustom.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
            End If
            txtS_chargers.Focus()
        End If
    End Sub


    Private Sub txtS_chargers_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtS_chargers.KeyUp
        Dim VALUE As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtS_chargers.Text) Then
                VALUE = txtS_chargers.Text
                txtS_chargers.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtS_chargers.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
            End If
            txtComm.Focus()
        End If
    End Sub

    Private Sub txtComm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtComm.KeyUp
        Dim VALUE As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtComm.Text) Then
                VALUE = txtComm.Text
                txtComm.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtComm.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
            End If
            txtTransport.Focus()
        End If
    End Sub

    Private Sub txtTransport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTransport.KeyUp
        Dim VALUE As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtTransport.Text) Then
                VALUE = txtTransport.Text
                txtTransport.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtTransport.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
            End If
            txtBank.Focus()
        End If
    End Sub

    Private Sub txtBank_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBank.KeyUp
        Dim VALUE As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtBank.Text) Then
                VALUE = txtBank.Text
                txtBank.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtBank.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
            End If
            txtVat.Focus()
        End If
    End Sub

    Private Sub txtVat_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtVat.KeyUp
        Dim VALUE As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtVat.Text) Then
                VALUE = txtVat.Text
                txtVat.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtVat.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
            End If
            txtNBT.Focus()
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If txtRemark.Text <> "" Then
        Else
            txtRemark.Text = "-"
        End If

        If txtUSD.Text <> "" Then
        Else
            MsgBox("Please enter the $ rate", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If

        If IsNumeric(txtUSD.Text) Then
        Else
            MsgBox("Please enter correct USD Rate", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================== tax
        If txtTax.Text <> "" Then
        Else
            txtTax.Text = "0"
        End If

        If IsNumeric(txtTax.Text) Then
        Else
            MsgBox("Please enter correct Tax Payment", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '=============================================== Custom Duty

        If txtCustom.Text <> "" Then
        Else
            txtCustom.Text = "0"
        End If

        If IsNumeric(txtCustom.Text) Then
        Else
            MsgBox("Please enter correct Custom Duty", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '=============================================== Shipping Charges
        If txtS_chargers.Text <> "" Then
        Else
            txtS_chargers.Text = "0"
        End If

        If IsNumeric(txtS_chargers.Text) Then
        Else
            MsgBox("Please enter correct Custom Duty", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================== Commission
        If txtComm.Text <> "" Then
        Else
            txtComm.Text = "0"
        End If

        If IsNumeric(txtComm.Text) Then
        Else
            MsgBox("Please enter correct Commission", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================= Transport
        If txtTransport.Text <> "" Then
        Else
            txtTransport.Text = "0"
        End If

        If IsNumeric(txtTransport.Text) Then
        Else
            MsgBox("Please enter correct Transport Chargers", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================= Bank Chargers
        If txtBank.Text <> "" Then
        Else
            txtBank.Text = "0"
        End If

        If IsNumeric(txtBank.Text) Then
        Else
            MsgBox("Please enter correct Bank Chargers", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '============================================ VAT
        If txtVat.Text <> "" Then
        Else
            txtVat.Text = "0"
        End If

        If IsNumeric(txtVat.Text) Then
        Else
            MsgBox("Please enter correct VAT", MsgBoxStyle.Information, "Information .......")
            Exit Sub
        End If
        '=========================================== NBT

        If txtNBT.Text <> "" Then
        Else
            txtNBT.Text = "0"
        End If

        If IsNumeric(txtNBT.Text) Then
        Else
            MsgBox("Please enter correct NBT", MsgBoxStyle.Information, "Information .......")
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
        Dim A As String
        Dim _sTRING As String
        Dim B As New ReportDocument
        Try
            _GETDATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)

            _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)


            nvcFieldList1 = "select * from M24GRN_Cost_Analysis_Header where  M24Ref_No='" & Trim(txtEntry.Text) & "'"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "update T01GRN_Header set T01Cost_Status='YES' where T01GRN_No='" & Trim(cboGRN.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                  " values('GRN_COST', 'EDIT','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE M24GRN_Cost_Analysis_Header SET M24GRN='" & Trim(cboGRN.Text) & "',M24Date='" & _GETDATE & "',M24USD_Rate='" & txtUSD.Text & "',M24Unit_Cost='" & txtU_Cost.Text & "',M24Tax='" & CDbl(txtTax.Text) & "',M24Cus_Deuty='" & CDbl(txtCustom.Text) & "',M24Shp_Chargers='" & CDbl(txtS_chargers.Text) & "',M24Comm='" & CDbl(txtComm.Text) & "',M24Transport='" & CDbl(txtTransport.Text) & "',M24Bank='" & CDbl(txtBank.Text) & "',M24VAT='" & CDbl(txtVat.Text) & "',M24NBT='" & CDbl(txtNBT.Text) & "',M24Remark='" & Trim(txtRemark.Text) & "',M24Status='A' WHERE M24Ref_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "DELETE FROM M23Cost_Analysis WHERE M23Ref_No='" & txtEntry.Text & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    nvcFieldList1 = "Insert Into M23Cost_Analysis(M23GRN,M23Ref_No,M23Item_Code,M23Rate,M23USD_Rate,M23Qty,M23Total,M23Status)" & _
                                                         " values('" & Trim(cboGRN.Text) & "','" & Trim(txtEntry.Text) & "', '" & (UltraGrid1.Rows(i).Cells(0).Value) & "','" & (UltraGrid1.Rows(i).Cells(3).Value) & "','" & (UltraGrid1.Rows(i).Cells(2).Value) & "','" & (UltraGrid1.Rows(i).Cells(4).Value) & "','" & CDbl(UltraGrid1.Rows(i).Cells(6).Value) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    i = i + 1
                Next

            Else
                Call Load_EntryNo()

                nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='COST' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "update T01GRN_Header set T01Cost_Status='YES' where T01GRN_No='" & Trim(cboGRN.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into M24GRN_Cost_Analysis_Header(M24GRN,M24Ref_No,M24Date,M24USD_Rate,M24Unit_Cost,M24Tax,M24Cus_Deuty,M24Shp_Chargers,M24Comm,M24Transport,M24Bank,M24VAT,M24NBT,M24Remark,M24Status)" & _
                                                                 " values('" & Trim(cboGRN.Text) & "', '" & Trim(txtEntry.Text) & "','" & _GETDATE & "','" & Trim(txtUSD.Text) & "','" & Trim(txtU_Cost.Text) & "','" & txtTax.Text & "','" & txtCustom.Text & "','" & txtS_chargers.Text & "','" & txtComm.Text & "','" & txtTransport.Text & "','" & txtBank.Text & "','" & txtVat.Text & "','" & txtNBT.Text & "','" & txtRemark.Text & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                   " values('GRN_COST', 'SAVE','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                i = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    nvcFieldList1 = "Insert Into M23Cost_Analysis(M23GRN,M23Ref_No,M23Item_Code,M23Rate,M23USD_Rate,M23Qty,M23Total,M23Status)" & _
                                                         " values('" & Trim(cboGRN.Text) & "','" & Trim(txtEntry.Text) & "', '" & (UltraGrid1.Rows(i).Cells(0).Value) & "','" & (UltraGrid1.Rows(i).Cells(3).Value) & "','" & (UltraGrid1.Rows(i).Cells(2).Value) & "','" & (UltraGrid1.Rows(i).Cells(4).Value) & "','" & CDbl(UltraGrid1.Rows(i).Cells(6).Value) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    i = i + 1
                Next

            End If

            transaction.Commit()
            connection.Close()
            A = MsgBox("Are you sure you want to print GRN Cost Report", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Information .......")
            If A = vbYes Then
                A = ConfigurationManager.AppSettings("ReportPath") + "\Cost.rpt"
                B.Load(A.ToString)
                B.SetDatabaseLogon("sa", "sainfinity")

                '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
                frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
                frmReport.CrystalReportViewer1.DisplayToolbar = True
                frmReport.CrystalReportViewer1.SelectionFormula = "{M24GRN_Cost_Analysis_Header.M24Ref_No} ='" & txtEntry.Text & "'"
                frmReport.Refresh()
                ' frmReport.CrystalReportViewer1.PrintReport()
                ' B.PrintToPrinter(1, True, 0, 0)
                frmReport.MdiParent = MDIMain
                frmReport.Show()
            End If

            Call Load_EntryNo()
            Call cLEAR_TEXT()
            Call Load_GRN()
            Me.txtU_Cost.Text = ""
            Me.cboGRN.Text = ""

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        frmview_Cost.Close()
        frmview_Cost.Show()
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim A As String
        Dim B As New ReportDocument
        Try
            A = ConfigurationManager.AppSettings("ReportPath") + "\Cost.rpt"
            B.Load(A.ToString)
            B.SetDatabaseLogon("sa", "sainfinity")

            '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
            frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
            frmReport.CrystalReportViewer1.DisplayToolbar = True
            frmReport.CrystalReportViewer1.SelectionFormula = "{M24GRN_Cost_Analysis_Header.M24Ref_No} ='" & txtEntry.Text & "'"
            frmReport.Refresh()
            ' frmReport.CrystalReportViewer1.PrintReport()
            ' B.PrintToPrinter(1, True, 0, 0)
            frmReport.MdiParent = MDIMain
            frmReport.Show()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)

            End If
        End Try

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
        Dim _GET_TIME As DateTime
        Dim GETDATE As DateTime

        Try
            GETDATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)

            _GET_TIME = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            A = MsgBox("Are you sure you want to delete this GRN Cost Report", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Information .......")
            If A = vbYes Then
                nvcFieldList1 = "UPDATE M24GRN_Cost_Analysis_Header SET M24Status='CANCEL' WHERE M24Ref_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE M23Cost_Analysis SET M23Status='CANCEL' WHERE M23Ref_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "UPDATE T01GRN_Header SET T01Cost_Status='NO' WHERE T01GRN_No='" & Trim(cboGRN.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)


                nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                  " values('GRN_COST', 'CANCEL','" & _GET_TIME & "','" & strDisname & "','" & Trim(txtEntry.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                MsgBox("Record deleted successfully", MsgBoxStyle.Information, "Information ......")
            End If
            transaction.Commit()
            connection.Close()
            Call cLEAR_TEXT()
            Call Load_EntryNo()
            Call Load_GRN()
            Me.txtU_Cost.Text = ""
            Me.cboGRN.Text = ""
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call cLEAR_TEXT()
        Call Load_EntryNo()
        Me.txtU_Cost.Text = ""
        Me.cboGRN.Text = ""
        Call Load_GRN()
    End Sub
End Class