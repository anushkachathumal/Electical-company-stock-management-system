Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Imports CrystalDecisions.CrystalReports.Engine
Public Class frmsales_Cnt
    Inherits System.Windows.Forms.Form
    Dim dsUser As DataSet
    Dim Clicked As String
    Dim c_dataCustomer1 As DataTable
    Dim _Root As String
    Dim _RefNo As String
    Dim _LogStaus As Boolean
    Dim _UserLevel As String
    Dim _AppUser As String
    Dim _cUSCODE As String

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Function Load_Root()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M02Root_Name as [##] from M02New_Root where  M02Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboRoot
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 296
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim Value As Double
        Dim i As Integer
        Dim _ST As String

        Try
            Sql = "select  *  from T12Transaction_Header inner join M05New_Customer on M05Cus_Code=T12Cus_Code INNER JOIN M02New_Root  ON M02Root_Code=T12Root  where  T12Tr_No='" & Trim(txtEntry.Text) & "'  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                txtDate.Text = M01.Tables(0).Rows(0)("T12Date")
                txtRef.Text = Trim(M01.Tables(0).Rows(0)("T12Ref_No"))
                txtRemark.Text = Trim(M01.Tables(0).Rows(0)("T12Remark"))
                cboRoot.Text = Trim(M01.Tables(0).Rows(0)("M02Root_Name"))
                cboCustomer.Text = Trim(M01.Tables(0).Rows(0)("M05Shop_Name"))
                cboVehicle.Text = Trim(M01.Tables(0).Rows(0)("T12V_No"))
                Value = Trim(M01.Tables(0).Rows(0)("T12Net_Amount"))
                txtNett.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtNett.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                Value = Trim(M01.Tables(0).Rows(0)("T12Dis_rate"))
                txtDis_Rate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtDis_Rate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                cmdEdit.Enabled = True
                cmdDelete.Enabled = True

            End If
            Call Load_Gride2()
            Sql = "select * from T13Transaction_Flutter inner join M11Product_Item on M11Part_No=T13Item_Code where T13Tr_No='" & Trim(txtEntry.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            i = 0
            For Each DTRow2 As DataRow In M01.Tables(0).Rows
                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(M01.Tables(0).Rows(i)("T13Item_Code"))
                newRow("Item Name") = Trim(M01.Tables(0).Rows(i)("M11Part_Name"))
                Value = Trim(M01.Tables(0).Rows(i)("T13Rate"))
                _ST = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                newRow("Rate") = _ST
                newRow("Qty") = CInt(M01.Tables(0).Rows(i)("T13Qty"))
                Value = Trim(M01.Tables(0).Rows(i)("T13Dis_Rate"))
                _ST = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

                newRow("Discount %") = _ST
                ' newRow("Rec.Qty") = txtRe_Qty.Text
                newRow("Free Issue") = CInt(M01.Tables(0).Rows(i)("T13Free_Issue"))
                Value = CDbl(M01.Tables(0).Rows(i)("T13Rate")) * CDbl(M01.Tables(0).Rows(i)("T13Qty"))
                Value = Value - (Value * CDbl(M01.Tables(0).Rows(i)("T13Dis_Rate")) / 100)
                _ST = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                _ST = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                newRow("Total") = _ST
                c_dataCustomer1.Rows.Add(newRow)
                txtCount.Text = UltraGrid1.Rows.Count

                i = i + 1
            Next
            Call Calculation_Net()
            Call Calculate_Final_Discount()

            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function
    Function Calculate_Final_Discount()
        On Error Resume Next
        Dim Value As Double

        If IsNumeric(txtNett.Text) And IsNumeric(txtDis_Rate.Text) Then
            Value = CDbl(txtNett.Text)
            Value = (Value * CDbl(txtDis_Rate.Text) / 100)
            txtDiscount.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
            txtDiscount.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

            Value = CDbl(txtNett.Text) - CDbl(txtDiscount.Text)
            txtGross.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
            txtGross.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
        End If
    End Function
    Function Load_Vehicle()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M06Vehi_No as [##] from M06Vehicle_Master where  M06Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboVehicle
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 142
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function


    Function Search_Root() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M02Root_Code from M02New_Root where  M02Status='A' and M02Root_Name='" & Trim(cboRoot.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                _Root = Trim(M01.Tables(0).Rows(0)("M02Root_Code"))
                Search_Root = True

                Sql = "select M05Shop_Name as [##] from M05New_Customer where M05Status='A' and M05Root_Code='" & _Root & "'"
                M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
                With cboCustomer
                    .DataSource = M01
                    .Rows.Band.Columns(0).Width = 311
                    '  .Rows.Band.Columns(1).Width = 160


                End With

            End If

          

            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Private Sub frmsales_Cnt_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        frmView_sales_cnt.Close()
    End Sub

    Private Sub frmsales_Cnt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_Root()
        txtDate.Text = Today
        txtTotal.ReadOnly = True
        txtTotal.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtRate.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtQty.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDiscount.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDiscount.ReadOnly = True
        txtFree.ReadOnly = True
        txtNett.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtDiscount.ReadOnly = True
        txtDiscount.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtGross.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtGross.ReadOnly = True
        txtCount.ReadOnly = True
        txtCount.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDis_Rate.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtFree.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        Call Load_Item_Code()
        Call Load_Item_Name()
        Call Load_EntryNo()
        Call Load_Gride2()
        Call Load_Vehicle()
        txtL_Dis.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDis_Rate.ReadOnly = True
        txtEntry.ReadOnly = True
        txtL_Dis.ReadOnly = True
    End Sub
    Function Load_EntryNo()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='DR'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtEntry.Text = "DR-00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtEntry.Text = "DR-0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtEntry.Text = "DR-" & M01.Tables(0).Rows(0)("P01No")
                End If
            End If

            con.ClearAllPools()
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function
    Private Sub cboRoot_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoot.AfterCloseUp
        Call Search_Root()
    End Sub

    Private Sub cboRoot_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboRoot.KeyUp
        If e.KeyCode = 13 Then
            cboVehicle.ToggleDropdown()
        End If
    End Sub

    Private Sub cboRoot_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoot.TextChanged
        Call Search_Root()
    End Sub

    Private Sub cboVehicle_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboVehicle.KeyUp
        If e.KeyCode = 13 Then
            txtRef.Focus()
        End If
    End Sub

    Private Sub txtRef_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRef.KeyUp
        If e.KeyCode = 13 Then
            cboCustomer.ToggleDropdown()
        End If
    End Sub

    Private Sub cboCustomer_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCustomer.AfterCloseUp
        If Search_Customer() = True Then
            Call Serch_Discount_Rate()
            Call Calculation_Net()
        End If
    End Sub

    Private Sub cboCustomer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCustomer.KeyUp
        If e.KeyCode = 13 Then
            cboCode.ToggleDropdown()

            If Search_Customer() = True Then
                Call Serch_Discount_Rate()
                Call Calculation_Net()
            End If
        End If
    End Sub

    Function Serch_Discount_Rate()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim Value As Double
        Dim _Firstdate As DateTime
        Dim _Monthlyincome As Double
        Dim _GET_DATE As DateTime
        Dim I As Integer

        Try
            Sql = "select * from M18Advance_Discount_Customer_Specific_Header where  M18Cus_Code='" & _cUSCODE & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Value = M01.Tables(0).Rows(0)("M18Total_Discount")
                txtDis_Rate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtDis_Rate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
            Else
                Sql = "select * from M01Setup_Company "
                M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
                If isValidDataset(M01) Then
                    Value = M01.Tables(0).Rows(0)("M01Sales_Discount")
                    txtDis_Rate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    txtDis_Rate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                End If
            End If

            _Firstdate = Month(txtDate.Text) & "/1/" & Year(txtDate.Text)
            _GET_DATE = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)
            _Monthlyincome = 0
            'CALCULATE MONTHLY SALES

            Sql = "select SUM(T12Net_Amount-(T12Net_Amount*(T12Dis_rate/100))) AS AMOUNT from T12Transaction_Header  WHERE T12Cus_Code='" & _cUSCODE & "' AND T12Date BETWEEN '" & _Firstdate & "' AND '" & _GET_DATE & "' AND T12Status='A' GROUP BY T12Cus_Code"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                _Monthlyincome = M01.Tables(0).Rows(0)("AMOUNT")
            End If

            If IsNumeric(txtNett.Text) Then
                _Monthlyincome = _Monthlyincome + CDbl(txtNett.Text)
            End If
            Sql = "SELECT * FROM M19Advance_Discount_Customer_Fluter WHERE M19Cus_Code='" & _cUSCODE & "' ORDER BY M19Amount DESC"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            I = 0
            For Each DTRow2 As DataRow In M01.Tables(0).Rows
                If _Monthlyincome >= CDbl(M01.Tables(0).Rows(I)("M19Amount")) Then
                    If IsNumeric(txtDis_Rate.Text) Then
                        txtDis_Rate.Text = CDbl(txtDis_Rate.Text) + CDbl(M01.Tables(0).Rows(I)("M19Discount"))
                    Else
                        txtDis_Rate.Text = M01.Tables(0).Rows(I)("M19Discount")
                    End If
                    Exit For
                End If

                I = I + 1
            Next

            '==========================================================================================
            Sql = "SELECT * FROM M17Company_Advance_Discount  ORDER BY M17Amount DESC"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            I = 0
            For Each DTRow2 As DataRow In M01.Tables(0).Rows
                If _Monthlyincome >= CDbl(M01.Tables(0).Rows(I)("M17Amount")) Then
                    If IsNumeric(txtDis_Rate.Text) Then
                        txtDis_Rate.Text = CDbl(txtDis_Rate.Text) + CDbl(M01.Tables(0).Rows(I)("M17Discount"))
                    Else
                        txtDis_Rate.Text = M01.Tables(0).Rows(I)("M17Discount")
                    End If
                    Exit For
                End If

                I = I + 1
            Next

            con.ClearAllPools()
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function
    Function Load_Item_Code()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M11Part_No as [##],M11Part_Name as [Item Name] from M11Product_Item where M11Status='A' and M11Type='PRODUCT ITEM' order by M11ID "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboCode
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 212
                .Rows.Band.Columns(1).Width = 310


            End With
            con.ClearAllPools()
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    
    Function Search_ItemName() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim Value As Double
        Try
            Sql = "select * from M11Product_Item where  M11Part_No='" & Trim(cboCode.Text) & "' and M11Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_ItemName = True
                cboItemName.Text = Trim(M01.Tables(0).Rows(0)("M11Part_Name"))
                Value = Trim(M01.Tables(0).Rows(0)("M11Retail_price"))
                txtRate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

            End If
            con.ClearAllPools()
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function
    Function Search_Itemcode() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim Value As Double
        Try
            Sql = "select * from M11Product_Item where  M11Part_Name='" & Trim(cboItemName.Text) & "' and M11Status='A' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Itemcode = True
                cboCode.Text = Trim(M01.Tables(0).Rows(0)("M11Part_No"))
                Value = Trim(M01.Tables(0).Rows(0)("M11Retail_price"))
                txtRate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
            End If
            con.ClearAllPools()
            con.close()


        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()

            End If
        End Try
    End Function
    Function Load_Item_Name()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M11Part_Name as [##] from M11Product_Item where M11Status='A' and M11Type='PRODUCT ITEM' order by M11ID "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboItemName
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 522
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Private Sub cboCode_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCode.AfterCloseUp
        Call Search_ItemName()
    End Sub

    Private Sub cboCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCode.KeyUp
        If e.KeyCode = 13 Then
            txtRate.Focus()
        End If
    End Sub


    Function Load_Gride2()
        Dim CustomerDataClass As New DAL_InterLocation()
        c_dataCustomer1 = CustomerDataClass.MakeDataTable_Sales_Cnt
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
           

            .DisplayLayout.Bands(0).Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            .DisplayLayout.Bands(0).Columns(4).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            .DisplayLayout.Bands(0).Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            '  .DisplayLayout.Bands(0).Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(1).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(2).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(3).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(4).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(5).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(6).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
     
        End With
    End Function

    Private Sub txtRate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRate.KeyUp
        Dim Value As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtRate.Text) Then
                Value = txtRate.Text
                txtRate.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtRate.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
            End If
            txtQty.Focus()
        End If

    End Sub

    Private Sub txtRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRate.TextChanged
        Call Calculation_Total()
    End Sub

    Function Calculation_Total()
        On Error Resume Next
        Dim Value As Double
        If IsNumeric(txtQty.Text) And IsNumeric(txtRate.Text) Then
            Value = CDbl(txtQty.Text) * CDbl(txtRate.Text)
        End If
        If IsNumeric(txtL_Dis.Text) Then
            Value = Value - (Value * CDbl(txtL_Dis.Text) / 100)

        End If
        txtTotal.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtTotal.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
    End Function

    Private Sub txtQty_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
        If e.KeyCode = 13 Then
            If txtQty.Text <> "" Then
                txtL_Dis.Focus()
            End If
        End If
    End Sub

    Private Sub txtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.TextChanged
        Call Calculation_Total()
    End Sub

    Private Sub txtL_Dis_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtL_Dis.KeyUp
        If e.KeyCode = 13 Then
            txtFree.Focus()
        End If
    End Sub

    Private Sub txtL_Dis_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtL_Dis.TextChanged
        Call Calculation_Total()
    End Sub

    Private Sub txtFree_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFree.KeyUp
        If e.KeyCode = 13 Then
            txtTotal.Focus()
        End If
    End Sub

    Private Sub txtTotal_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTotal.KeyUp
        Try
            If e.KeyCode = 13 Then
                If Search_ItemName() = True Then
                Else
                    MsgBox("Please enter the Correct Item Code", MsgBoxStyle.Information, "Information ........")
                    cboCode.ToggleDropdown()
                    Exit Sub
                End If

                If txtRate.Text <> "" Then
                Else
                    MsgBox("Please enter the Rate", MsgBoxStyle.Information, "Information ........")
                    txtRate.Focus()
                    Exit Sub
                End If
                If IsNumeric(txtRate.Text) Then
                Else
                    MsgBox("Please enter the correct Rate", MsgBoxStyle.Information, "Information ........")
                    txtRate.Focus()
                    Exit Sub
                End If
                '-----------------------------------------------------------------------------------------
                If txtQty.Text <> "" Then
                Else
                    txtQty.Text = "0"
                End If
                If IsNumeric(txtQty.Text) Then
                Else
                    MsgBox("Please enter the correct Qty", MsgBoxStyle.Information, "Information ........")
                    txtQty.Focus()
                    Exit Sub
                End If
                '------------------------------------------------------------------------------------------
                If txtL_Dis.Text <> "" Then
                Else
                    txtL_Dis.Text = "0"
                End If
                If IsNumeric(txtL_Dis.Text) Then
                Else
                    MsgBox("Please enter the correct Discount", MsgBoxStyle.Information, "Information ........")
                    txtDiscount.Focus()
                    Exit Sub
                End If

                If txtFree.Text <> "" Then
                Else
                    txtFree.Text = "0"
                End If

                If IsNumeric(txtFree.Text) Then
                Else
                    txtFree.Text = "0"
                End If

                Call Calculation_Total()
                Dim newRow As DataRow = c_dataCustomer1.NewRow
                newRow("Item Code") = Trim(cboCode.Text)
                newRow("Item Name") = cboItemName.Text
                newRow("Rate") = txtRate.Text
                newRow("Qty") = txtQty.Text
                newRow("Discount %") = txtL_Dis.Text
                ' newRow("Rec.Qty") = txtRe_Qty.Text
                newRow("Free Issue") = txtFree.Text
                newRow("Total") = txtTotal.Text
                c_dataCustomer1.Rows.Add(newRow)
                txtCount.Text = UltraGrid1.Rows.Count
                Call Calculation_Net()
                Call Serch_Discount_Rate()
                Call Calculate_Final_Discount()
                Me.txtFree.Text = ""
                Me.txtTotal.Text = "00.00"
                Me.txtL_Dis.Text = "0"
                Me.txtQty.Text = ""
                Me.txtRate.Text = ""
                Me.cboCode.Text = ""
                Me.cboItemName.Text = ""
                cboCode.ToggleDropdown()
            End If
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
               
            End If
        End Try
    End Sub

    Function Calculation_Net()
        On Error Resume Next
        Dim i As Integer
        Dim Value As Double

        For Each uRow As UltraGridRow In UltraGrid1.Rows
            ' MsgBox(Double.TryParse(txtNett.Text, value))
            Value = Value + CDbl((UltraGrid1.Rows(i).Cells(6).Value))
            i = i + 1
        Next
        txtNett.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtNett.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
        If IsNumeric(txtDis_Rate.Text) Then
            Value = Value - (Value * CDbl(txtDis_Rate.Text / 100))
        End If
        txtGross.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtGross.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))

    End Function

    Private Sub UltraGrid1_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid1.AfterRowsDeleted
        Call Calculation_Net()
        Call Calculate_Final_Discount()
    End Sub

    Private Sub txtDis_Rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDis_Rate.TextChanged
        Call Calculate_Final_Discount()
    End Sub

    Function Search_Customer() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim i As Integer

        Try
            Sql = "select * from M05New_Customer where M05Status='A' and M05Root_Code='" & _Root & "' and M05Shop_Name='" & Trim(cboCustomer.Text) & "' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                _cUSCODE = Trim(M01.Tables(0).Rows(0)("M05Cus_Code"))
                _RefNo = Trim(M01.Tables(0).Rows(0)("M05Ref_Code"))
                Search_Customer = True
            End If

            'If IsNumeric(txtGross.Text) Then
            '    Sql = ""
            'End If
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Function Search_Vehicle_no() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select * from M06Vehicle_Master where M06Status='A' and M06Vehi_No='" & Trim(cboVehicle.Text) & "'  "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then

                Search_Vehicle_no = True
            End If
            con.ClearAllPools()
            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If
        End Try
    End Function

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Sql = "select * from M02New_Root where M02Root_Name='" & Trim(cboRoot.Text) & "' and M02Status='A'"
        M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
        If isValidDataset(M01) Then
            _Root = Trim(M01.Tables(0).Rows(0)("M02Root_Code"))
        Else
            MsgBox("Please enter the correcrt Root Name", MsgBoxStyle.Information, "Information ........")
            cboRoot.ToggleDropdown()
            con.ClearAllPools()
            con.close()
            Exit Sub
        End If

        If Search_Customer() = True Then
        Else
            MsgBox("Please select the correct customer", MsgBoxStyle.Information, "Information ......")
            cboCustomer.ToggleDropdown()
            Exit Sub
        End If

        If Search_Vehicle_no() = True Then
        Else
            MsgBox("Please select the correct Vehicle No", MsgBoxStyle.Information, "Information ......")
            cboVehicle.ToggleDropdown()
            Exit Sub
        End If

        If Trim(txtRef.Text) <> "" Then
        Else
            MsgBox("Please enter the Referance No", MsgBoxStyle.Information, "Information ...........")
            txtRef.Focus()
            Exit Sub
        End If

        If txtDis_Rate.Text <> "" Then
        Else
            txtDis_Rate.Text = "0"
        End If
        If IsNumeric(txtDis_Rate.Text) Then
        Else
            MsgBox("Please enter the discount rate", MsgBoxStyle.Information, "Information .......")
            txtDis_Rate.Focus()
            Exit Sub
        End If

        If UltraGrid1.Rows.Count > 0 Then
        Else
            MsgBox("Please enter the Items ", MsgBoxStyle.Information, "Information .......")
            cboCode.ToggleDropdown()
            Exit Sub
        End If

        If Trim(txtRemark.Text) <> "" Then
        Else
            txtRemark.Text = "-"
        End If
        con.ClearAllPools()
        con.close()
        Call Save_Data()
    End Sub

    Function Save_Data()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean
        'connection.ClearAllPools()
        SqlConnection.ClearAllPools()
        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim _GetDate As DateTime
        Dim _Get_Time As DateTime
        Dim I As Integer
        Dim A As String
        Dim B As New ReportDocument
        Dim _remark As String

        Try
            _GetDate = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)
            _Get_Time = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            nvcFieldList1 = "SELECT * FROM T12Transaction_Header WHERE T12Tr_No='" & Trim(txtEntry.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                MsgBox("This Sales Invoice no alrady exsist", MsgBoxStyle.Information, "Information ......")
                connection.ClearAllPools()
                connection.Close()
                Exit Function

            Else
                Call Load_EntryNo()

                nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='DR' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into T12Transaction_Header(T12Tr_No,T12Tr_Type,T12Ref_No,T12Date,T12Remark,T12Cus_Code,T12Sales_Ref,T12Net_Amount,T12Dis_rate,T12Status,T12Root,T12V_No)" & _
                                                                  " values('" & Trim(txtEntry.Text) & "','SALES', '" & Trim(txtRef.Text) & "','" & _GetDate & "','" & Trim(txtRemark.Text) & "','" & _cUSCODE & "','" & _RefNo & "','" & Trim(txtNett.Text) & "','" & txtDis_Rate.Text & "','A','" & _Root & "','" & Trim(cboVehicle.Text) & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                " values('SALES','SAVE', '" & Now & "','" & strDisname & "','" & txtEntry.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                '=================================================================================
                'PAYMENY
                nvcFieldList1 = "Insert Into T14Pay_Main(T14Ref_No,T14Tr_Type,T14Date,T14Tr_No,T14Cash,T14Credit,T14Chq,T14Status)" & _
                                                              " values('" & txtEntry.Text & "','SALES', '" & _GetDate & "','-','0','" & Trim(txtGross.Text) & "','0','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                '==================================================================================
                'Outstanding
                _remark = "CREDIT INVOICE - " & Trim(txtEntry.Text) & "/" & Trim(txtRef.Text)
                nvcFieldList1 = "Insert Into T16OutStanding(T16Date,T16Ref_No,T16Invoice_No,T16Tr_Type,T16Cus_Code,T16Remark,T16CR,T16DR,T16Status,T16Inv_Discount,T16Cash_Dis,T16Chq_Dis,T16Pay_no)" & _
                                                     " values('" & _GetDate & "','" & Trim(txtEntry.Text) & "', '" & Trim(txtRef.Text) & "','SALES','" & _cUSCODE & "','" & _remark & "','" & CDbl(txtGross.Text) & "','0','A','0','0','0','-')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                '==================================================================================
                'SALES COMMISSION
                nvcFieldList1 = "SELECT * FROM M01Setup_Company WHERE M01Ref_No='CE'"
                MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                If isValidDataset(MB51) Then
                    nvcFieldList1 = "Insert Into T15Sales_Commission(T15Date,T15Invoice_No,T15Tr_Type,T15Cus_Code,T15Net_Amount,T15Chq_no,T15Cash_Com,T15Chq_Com,T15Collection_Com,T15Status)" & _
                                                " values('" & _GetDate & "','" & Trim(txtEntry.Text) & "', 'SALES','" & _cUSCODE & "','" & CDbl(txtGross.Text) & "','-','" & MB51.Tables(0).Rows(0)("M01Sales_Comm") & "','0','0','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                End If


                I = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    nvcFieldList1 = "Insert Into T13Transaction_Flutter(T13Tr_No,T13Item_Code,T13Qty,T13Free_Issue,T13Rate,T13Dis_Rate,T13Status)" & _
                                                             " values('" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & Trim(UltraGrid1.Rows(I).Cells(3).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(5).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(2).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(4).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '==================================================================
                    'UPDATE FROM LOCATION
                    nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                          " values('" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & Trim(cboVehicle.Text) & "','" & _GetDate & "','SALES','" & Trim(txtEntry.Text) & "','" & -(UltraGrid1.Rows(I).Cells(3).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '==================================================================
                    ''UPDATE TO LOCATION 
                    'If Trim(UltraGrid2.Rows(I).Cells(0).Text) = "USABLE" Then
                    '    nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                    '                                      " values('" & Trim(UltraGrid2.Rows(I).Cells(1).Text) & "', '" & _Location & "','" & _GetDate & "','UNLOADING','" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid2.Rows(I).Cells(5).Text) & "','A')"
                    '    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    'End If
                    I = I + 1
                Next
            End If
            MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.ClearAllPools()
            connection.Close()
            'A = MsgBox("Are you sure you want to print this Unloading Report", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Information .........")
            'If A = vbYes Then
            '    A = ConfigurationManager.AppSettings("ReportPath") + "\Unloading.rpt"
            '    B.Load(A.ToString)
            '    B.SetDatabaseLogon("sa", "sainfinity")

            '    '  frmReport.CrystalReportViewer1.SelectionFormula = "{T01Transaction_Header.T01RefNo} =" & P01 & ""
            '    frmReport.CrystalReportViewer1.ReportSource = B 'intanance System\CrystalReport1" 'B ' "f:\salesinvo1.rpt" 'A.ToString '"F:\Stock Maintanance System\Report\salesinvo1.rpt"
            '    frmReport.CrystalReportViewer1.DisplayToolbar = True
            '    frmReport.CrystalReportViewer1.SelectionFormula = "{{T23Unloading_Header.T23Tr_No}='" & Trim(txtEntry.Text) & "'"
            '    frmReport.Refresh()
            '    ' frmReport.CrystalReportViewer1.PrintReport()
            '    ' B.PrintToPrinter(1, True, 0, 0)
            '    frmReport.MdiParent = MDIMain
            '    frmReport.Show()
            'End If
            Call Clear_text()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.ClearAllPools()
                connection.Close()
            End If
        End Try
    End Function

    Function cLEAR_TEXT()
        Me.txtDis_Rate.Text = ""
        Me.txtDiscount.Text = ""
        Me.txtFree.Text = ""
        Me.txtGross.Text = ""
        Me.txtL_Dis.Text = ""
        Me.txtGross.Text = ""
        Me.cboCustomer.Text = ""
        Me.txtRate.Text = ""
        Me.cboVehicle.Text = ""
        Me.cboItemName.Text = ""
        Me.cboCode.Text = ""
        Me.txtDis_Rate.Text = ""
        Me.txtRef.Text = ""
        Me.txtCount.Text = ""
        Me.txtNett.Text = ""
        _LogStaus = False
        cmdEdit.Enabled = False
        cmdDelete.Enabled = False
        Call Load_Gride2()
        Call Load_EntryNo()
        OPRUser.Visible = False
    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call cLEAR_TEXT()
        Me.cboRoot.Text = ""
    End Sub

    Private Sub DeactivateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactivateToolStripMenuItem.Click
        frmView_sales_cnt.Close()
        frmView_sales_cnt.Show()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        If _LogStaus = False Then
            OPRUser.Visible = True
            txtPassword.Text = ""
            txtUserName.Text = ""
            txtUserName.Focus()
        Else


            Sql = "select * from M02New_Root where M02Root_Name='" & Trim(cboRoot.Text) & "' and M02Status='A'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                _Root = Trim(M01.Tables(0).Rows(0)("M02Root_Code"))
            Else
                MsgBox("Please enter the correcrt Root Name", MsgBoxStyle.Information, "Information ........")
                cboRoot.ToggleDropdown()
                con.ClearAllPools()
                con.close()
                Exit Sub
            End If

            If Search_Customer() = True Then
            Else
                MsgBox("Please select the correct customer", MsgBoxStyle.Information, "Information ......")
                cboCustomer.ToggleDropdown()
                Exit Sub
            End If

            If Search_Vehicle_no() = True Then
            Else
                MsgBox("Please select the correct Vehicle No", MsgBoxStyle.Information, "Information ......")
                cboVehicle.ToggleDropdown()
                Exit Sub
            End If

            If Trim(txtRef.Text) <> "" Then
            Else
                MsgBox("Please enter the Referance No", MsgBoxStyle.Information, "Information ...........")
                txtRef.Focus()
                Exit Sub
            End If

            If txtDis_Rate.Text <> "" Then
            Else
                txtDis_Rate.Text = "0"
            End If
            If IsNumeric(txtDis_Rate.Text) Then
            Else
                MsgBox("Please enter the discount rate", MsgBoxStyle.Information, "Information .......")
                txtDis_Rate.Focus()
                Exit Sub
            End If

            If UltraGrid1.Rows.Count > 0 Then
            Else
                MsgBox("Please enter the Items ", MsgBoxStyle.Information, "Information .......")
                cboCode.ToggleDropdown()
                Exit Sub
            End If

            If Trim(txtRemark.Text) <> "" Then
            Else
                txtRemark.Text = "-"
            End If
            con.ClearAllPools()
            con.close()

            Call Edit_Data()
        End If
    End Sub

    Function Edit_Data()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean
        'connection.ClearAllPools()
        SqlConnection.ClearAllPools()
        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim _GetDate As DateTime
        Dim _Get_Time As DateTime
        Dim I As Integer
        Dim A As String
        Dim B As New ReportDocument
        Dim _remark As String
        Dim m01 As DataSet

        Try
            _GetDate = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text)
            _Get_Time = Month(txtDate.Text) & "/" & Microsoft.VisualBasic.Day(txtDate.Text) & "/" & Year(txtDate.Text) & " " & Hour(Now) & ":" & Minute(Now)

            nvcFieldList1 = "SELECT * FROM T12Transaction_Header WHERE T12Tr_No='" & Trim(txtEntry.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then

                nvcFieldList1 = "SELECT * FROM T25Outstanding_Collection WHERE T25Inv_No='" & Trim(txtEntry.Text) & "' AND T25Status='PAID'"
                m01 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                If isValidDataset(m01) Then
                    MsgBox("This Invoice alrady paid", MsgBoxStyle.Information, "Information .......")
                    connection.ClearAllPools()
                    connection.Close()
                    Exit Function
                End If
                nvcFieldList1 = "UPDATE T12Transaction_Header SET T12Date='" & _GetDate & "',T12Remark='" & Trim(txtRemark.Text) & "',T12Cus_Code='" & _cUSCODE & "',T12Sales_Ref='" & _RefNo & "',T12Net_Amount='" & CDbl(txtNett.Text) & "',T12Dis_rate='" & txtDis_Rate.Text & "',T12Status='A',T12Root='" & _Root & "',T12V_No='" & Trim(cboVehicle.Text) & "',T12Ref_No='" & Trim(txtRef.Text) & "' WHERE T12Tr_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                " values('SALES','EDIT', '" & Now & "','" & _AppUser & "','" & txtEntry.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                '=================================================================================
                'PAYMENY

                nvcFieldList1 = "UPDATE T14Pay_Main SET T14Date='" & _GetDate & "',T14Cash='" & CDbl(txtGross.Text) & "',T14Status='A' WHERE T14Ref_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                '==================================================================================
                'Outstanding
                _remark = "CREDIT INVOICE - " & Trim(txtEntry.Text) & "/" & Trim(txtRef.Text)
                nvcFieldList1 = "UPDATE T16OutStanding SET T16Date='" & _GetDate & "',T16Cus_Code='" & _cUSCODE & "',T16Invoice_No='" & Trim(txtRef.Text) & "',T16CR='" & CDbl(txtGross.Text) & "',T16Status='A' WHERE T16Ref_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                '==================================================================================
                'SALES COMMISSION
                nvcFieldList1 = "SELECT * FROM M01Setup_Company WHERE M01Ref_No='CE'"
                MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                If isValidDataset(MB51) Then
                    'nvcFieldList1 = "Insert Into T15Sales_Commission(T15Date,T15Invoice_No,T15Tr_Type,T15Cus_Code,T15Net_Amount,T15Chq_no,T15Cash_Com,T15Chq_Com,T15Collection_Com,T15Status)" & _
                    '                            " values('" & _GetDate & "','" & Trim(txtEntry.Text) & "', 'SALES','" & _cUSCODE & "','" & CDbl(txtGross.Text) & "','-','" & MB51.Tables(0).Rows(0)("M01Sales_Comm") & "','0','0','A')"
                    'ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                    nvcFieldList1 = "UPDATE T15Sales_Commission SET T15Cus_Code='" & _cUSCODE & "',T15Date='" & _GetDate & "',T15Net_Amount='" & CDbl(txtGross.Text) & "',T15Cash_Com='" & MB51.Tables(0).Rows(0)("M01Sales_Comm") & "',T15Status='A' WHERE T15Invoice_No='" & Trim(txtEntry.Text) & "'"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                End If

                nvcFieldList1 = "DELETE FROM T13Transaction_Flutter WHERE T13Tr_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "DELETE FROM S01Stock_Product_Items WHERE S01Ref_No='" & Trim(txtEntry.Text) & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                I = 0
                For Each uRow As UltraGridRow In UltraGrid1.Rows
                    nvcFieldList1 = "Insert Into T13Transaction_Flutter(T13Tr_No,T13Item_Code,T13Qty,T13Free_Issue,T13Rate,T13Dis_Rate,T13Status)" & _
                                                             " values('" & Trim(txtEntry.Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & Trim(UltraGrid1.Rows(I).Cells(3).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(5).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(2).Text) & "','" & Trim(UltraGrid1.Rows(I).Cells(4).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '==================================================================
                    'UPDATE FROM LOCATION
                    nvcFieldList1 = "Insert Into S01Stock_Product_Items(S01Part_No,S01Loc_Code,S01Date,S01Tr_Type,S01Ref_No,S01Qty,S01Status)" & _
                                                          " values('" & Trim(UltraGrid1.Rows(I).Cells(0).Text) & "', '" & Trim(cboVehicle.Text) & "','" & _GetDate & "','SALES','" & Trim(txtEntry.Text) & "','" & -(UltraGrid1.Rows(I).Cells(3).Text) & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                    '==================================================================
                   
                    I = I + 1
                Next
            End If
            MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.ClearAllPools()
            connection.Close()
            
            Call cLEAR_TEXT()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.ClearAllPools()
                connection.Close()
            End If
        End Try
    End Function
    Private Sub txtUserName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUserName.KeyUp
        If e.KeyCode = 13 Then
            txtPassword.Focus()
        End If
    End Sub

    Private Sub txtPassword_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPassword.KeyUp
        If e.KeyCode = 13 Then
            OK.Focus()
        End If
    End Sub

    Private Sub cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancel.Click
        OPRUser.Visible = False
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim SQL As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        Dim M01 As DataSet

        Try
            con = DBEngin.GetConnection()
            SQL = "SELECT * FROM X02User_Loging WHERE (X02User_Name ='" & txtUserName.Text & "')and X02Password='" & txtPassword.Text & "' and X02User_Level in ('ADMIN','MANEGER','DIRECTOR') "
            M01 = DBEngin.ExecuteDataset(con, Nothing, SQL)
            If isValidDataset(M01) Then
                _LogStaus = True
                _AppUser = Trim(txtUserName.Text)
                OPRUser.Visible = False
            Else
                MsgBox("User name and pasword combination not found", MsgBoxStyle.Information, "Information ......")
                txtUserName.Focus()
                con.ClearAllPools()
                con.close()
                Exit Sub
            End If
            con.ClearAllPools()
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()
            End If

        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim A As String

        If _LogStaus = False Then
            OPRUser.Visible = True
            txtPassword.Text = ""
            txtUserName.Text = ""
            txtUserName.Focus()
        Else
            A = MsgBox("Are you sure you want to delete this invoice", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Delete Invoice .....")
            If A = vbYes Then
                Call Deactive_Data()
            End If
        End If
    End Sub

    Function Deactive_Data()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean
        'connection.ClearAllPools()
        SqlConnection.ClearAllPools()
        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Try
            nvcFieldList1 = "UPDATE T12Transaction_Header SET T12Status='CANCEL' WHERE T12Tr_No='" & Trim(txtEntry.Text) & "'"
            ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            nvcFieldList1 = "UPDATE T13Transaction_Flutter SET T13Status='CANCEL' WHERE T13Tr_No='" & Trim(txtEntry.Text) & "'"
            ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            nvcFieldList1 = "UPDATE T14Pay_Main SET T14Status='CANCEL' WHERE T14Ref_No='" & Trim(txtEntry.Text) & "'"
            ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            nvcFieldList1 = "UPDATE T15Sales_Commission SET T15Status='CANCEL' WHERE T15Invoice_No='" & Trim(txtEntry.Text) & "'"
            ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            nvcFieldList1 = "UPDATE T16OutStanding SET T16Status='CANCEL' WHERE T16Ref_No='" & Trim(txtEntry.Text) & "'"
            ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            MsgBox("Invoice deleted successfully", MsgBoxStyle.Information, "Information ......")

            transaction.Commit()
            connection.ClearAllPools()
            connection.Close()
           
            Call cLEAR_TEXT()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.ClearAllPools()
                connection.Close()
            End If
        End Try
    End Function


End Class