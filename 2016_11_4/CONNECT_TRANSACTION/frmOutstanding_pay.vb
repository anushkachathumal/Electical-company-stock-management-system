Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Public Class frmOutstanding_pay
    Dim _Locstatus As Boolean
    Dim _FindStatus As Boolean
    Dim c_dataCustomer1 As DataTable
    Dim _cuscode As String
    Dim _Root As String
    Dim _RefNo As String
    Dim _SALES_COM As Double
    Dim _CASH_COM As Double
    Dim _CHQ_COM As Double

    Function Load_EntryNo()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='OU'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtEntry.Text = "C-00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtEntry.Text = "C-0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtEntry.Text = "C-" & M01.Tables(0).Rows(0)("P01No")
                End If
            End If

            con.ClearAllPools()
            con.CLOSE()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.CLOSE()
            End If
        End Try
    End Function

    Private Sub frmOutstanding_pay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtEntry.ReadOnly = True
        txtEntry.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtDate1.Text = Today
        '  txtDOR.Text = Today
        txtCash1.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtPayment1.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtPayment1.ReadOnly = True
        txtAmount1.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        Call Load_Root()
        Call Load_EntryNo()
        Call Load_PAYTEARMS()
        txtInv_Amount.ReadOnly = True
        txtInv_Amount.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtCash1.ReadOnly = True
        cboBank1.ReadOnly = True
        txtChq1.ReadOnly = True
        txtAmount1.ReadOnly = True
        Call Load_BANK()
        Call Load_Gride_Invo()
        txtDiscount.ReadOnly = True
        txtDiscount.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        txtAmount1.Text = "00.00"
        txtCash1.Text = "00.00"
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
                .Rows.Band.Columns(0).Width = 400
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

    Function SEARCH_INVOICE() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Dim VALUE As Double
        Try
            Sql = "select (max(T16CR)-sum(T16DR)) as [Paid_Amount] from View_Outstanding_Balance where  T16Cus_Code='" & _cuscode & "' AND T16Ref_No='" & Trim(cboInvoice.Text) & "' group by T16Cus_Code,T16Ref_No having sum(T16CR-T16DR)>0"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                SEARCH_INVOICE = True
                VALUE = M01.Tables(0).Rows(0)("Paid_Amount")
                txtInv_Amount.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtInv_Amount.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
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

    Function Load_BANK()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M22Description as [##] from M22Common where  M22Ref='4'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboBank1
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 169
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


    Function Load_Invoice()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select T16Ref_No as [##],max(T16Invoice_No) as [Invoice No],REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,max(T16CR)),1), '.0000','') as [inv.Amount],REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,sum(T16DR)),1), '.0000','') as [Paid Amount] from View_Outstanding_Balance where  T16Cus_Code='" & _cuscode & "' group by T16Cus_Code,T16Ref_No having sum(T16CR-T16DR)>0"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboInvoice
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 207
                .Rows.Band.Columns(1).Width = 110
                .Rows.Band.Columns(2).Width = 90
                .Rows.Band.Columns(3).Width = 90
                .Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                .Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
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

    Function Load_PAYTEARMS()
        Dim Sql As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet

        Try
            Sql = "select M22Description as [##] from M22Common where  M22Ref='3' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboPay_tearms
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 169
                '.Rows.Band.Columns(1).Width = 110
                '.Rows.Band.Columns(2).Width = 90
                '.Rows.Band.Columns(3).Width = 90
                '.Rows.Band.Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
                '.Rows.Band.Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
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
                    .Rows.Band.Columns(0).Width = 400
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


    Private Sub cboRoot_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoot.AfterCloseUp
        Call Search_Root()
    End Sub

    Private Sub cboRoot_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboRoot.KeyUp
        If e.KeyCode = 13 Then
            Call Search_Root()
            cboCustomer.ToggleDropdown()
        End If
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

            Sql = "SELECT * FROM M01Setup_Company"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                _SALES_COM = M01.Tables(0).Rows(0)("M01Sales_Comm")
            End If

            Sql = "SELECT * FROM M18Advance_Discount_Customer_Specific_Header WHERE M18Cus_Code='" & _cuscode & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                _CASH_COM = M01.Tables(0).Rows(0)("M18Cash_Discount")
            Else
                Sql = "SELECT * FROM M01Setup_Company"
                M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
                If isValidDataset(M01) Then
                    _CASH_COM = M01.Tables(0).Rows(0)("M01Cash_Collection")
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
    Function Calculate_Net()
        On Error Resume Next
        Dim Value As Double
        If IsNumeric(txtDiscount.Text) Then
            Value = txtDiscount.Text
        End If
        Value = CDbl(txtInv_Amount.Text) - Value
        txtPayment1.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
        txtPayment1.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
    End Function
    Private Sub cboCustomer_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCustomer.AfterCloseUp
        Call Search_Customer()
        Call Load_Invoice()
    End Sub

    Private Sub cboCustomer_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCustomer.KeyUp
        If e.KeyCode = 13 Then
            Call Search_Customer()
            Call Load_Invoice()
            cboInvoice.ToggleDropdown()
        End If
    End Sub

    Private Sub cboInvoice_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboInvoice.AfterCloseUp
        Call SEARCH_INVOICE()
    End Sub

    Private Sub cboInvoice_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboInvoice.KeyUp
        If e.KeyCode = 13 Then
            Call SEARCH_INVOICE()
            cboPay_tearms.ToggleDropdown()
        End If
    End Sub

    Private Sub cboPay_tearms_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPay_tearms.AfterCloseUp
        Call Calculate_Net()
    End Sub

    Private Sub cboPay_tearms_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboPay_tearms.KeyUp
        Dim vALUE As Double

        If e.KeyCode = 13 Then
            If Trim(cboPay_tearms.Text) = "CASH/FULL PAY" Then
                If IsNumeric(txtInv_Amount.Text) Then
                    vALUE = txtInv_Amount.Text
                    vALUE = (vALUE * (_CASH_COM / 100))
                    txtDiscount.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    txtDiscount.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
                End If
                txtCash1.Focus()
            ElseIf Trim(cboPay_tearms.Text) = "CASH PAY" Then
                txtDiscount.Text = "0.00"
                txtPayment1.Text = txtInv_Amount.Text
                txtCash1.Focus()
            Else
                txtDiscount.Text = "0.00"
                txtPayment1.Text = txtInv_Amount.Text
                cboBank1.ToggleDropdown()
            End If
            Call Calculate_Net()
        End If
    End Sub

    Private Sub cboPay_tearms_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPay_tearms.TextChanged
        Dim vALUE As Double
        If Trim(cboPay_tearms.Text) = "CASH/FULL PAY" Then
            If IsNumeric(txtInv_Amount.Text) Then
                vALUE = txtInv_Amount.Text
                vALUE = (vALUE * (_CASH_COM / 100))
                txtDiscount.Text = (vALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtDiscount.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", vALUE))
            End If
            txtCash1.ReadOnly = False
        ElseIf Trim(cboPay_tearms.Text) = "CHEQUE" Then
            cboBank1.ReadOnly = False
            txtChq1.ReadOnly = False
            txtDOR.ReadOnly = False
            txtAmount1.ReadOnly = False
            txtDiscount.Text = ""
            txtPayment1.Text = txtInv_Amount.Text
        ElseIf Trim(cboPay_tearms.Text) = "CASH PAY" Then
            txtCash1.ReadOnly = False
            txtDiscount.Text = ""
            txtPayment1.Text = txtInv_Amount.Text
        End If

        Call Calculate_Net()
    End Sub

    Private Sub txtCash1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCash1.KeyUp
        Dim Value As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtCash1.Text) Then
                If CDbl(txtCash1.Text) > 0 Then
                    Value = CDbl(txtCash1.Text)
                    txtCash1.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                    txtCash1.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                End If
            End If
            cmdRecord.Focus()
        End If
    End Sub


    Private Sub cboBank1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboBank1.KeyUp
        If e.KeyCode = 13 Then
            txtChq1.Focus()
        End If
    End Sub

    Private Sub txtChq1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChq1.KeyUp
        If e.KeyCode = 13 Then
            txtDOR.Focus()
        End If
    End Sub

    Private Sub txtDOR_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDOR.KeyUp
        If e.KeyCode = 13 Then
            txtAmount1.Focus()
        End If
    End Sub

    Private Sub txtAmount1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAmount1.KeyUp
        Dim Value As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtAmount1.Text) Then
                Value = txtAmount1.Text
                txtAmount1.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtAmount1.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
            End If
            cmdRecord.Focus()
        End If
    End Sub

    Function Load_Gride_Invo()
        Dim CustomerDataClass As New DAL_InterLocation()
        c_dataCustomer1 = CustomerDataClass.MakeDataTable_Inv
        UltraGrid3.DataSource = c_dataCustomer1
        With UltraGrid3
            .DisplayLayout.Bands(0).Columns(0).Width = 80
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False
            .DisplayLayout.Bands(0).Columns(1).Width = 90
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False
            .DisplayLayout.Bands(0).Columns(2).Width = 90
            .DisplayLayout.Bands(0).Columns(2).AutoEdit = False

            .DisplayLayout.Bands(0).Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            .DisplayLayout.Bands(0).Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            '.DisplayLayout.Bands(0).Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            '.DisplayLayout.Bands(0).Columns(9).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right

        End With
    End Function

    Private Sub txtAmount1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmount1.TextChanged
        Dim VALUE As Double

        If IsNumeric(txtAmount1.Text) Then
            If CDbl(txtAmount1.Text) > 0 Then
                VALUE = CDbl(txtAmount1.Text)
                txtPayment1.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtPayment1.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
            End If
        End If
    End Sub

    'Private Sub txtCash1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCash1.TextChanged
    '    Dim VALUE As Double

    '    If IsNumeric(txtCash1.Text) Then
    '        VALUE = CDbl(txtCash1.Text)
    '        txtPayment1.Text = (VALUE.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
    '        txtPayment1.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", VALUE))
    '    End If
    'End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

  
    Private Sub cmdRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRecord.Click
        Dim Value As Double

        If SEARCH_INVOICE() = True Then
        Else
            MsgBox("Please enter the correct Invoice No", MsgBoxStyle.Information, "Information .......")
            cboInvoice.ToggleDropdown()
            Exit Sub
        End If

        If Trim(cboPay_tearms.Text) <> "" Then
        Else
            MsgBox("Please select the payment type", MsgBoxStyle.Information, "Information .........")
            cboPay_tearms.ToggleDropdown()
            Exit Sub
        End If

        If Trim(cboPay_tearms.Text) = "CASH PAY" Or Trim(cboPay_tearms.Text) = "CASH/FULL PAY" Then
            If txtCash1.Text <> "" Then
            Else
                MsgBox("Please enter the cash amount", MsgBoxStyle.Information, "Information ........")
                txtCash1.Focus()
                Exit Sub
            End If

            If IsNumeric(txtCash1.Text) Then
            Else
                MsgBox("Please enter the correct Cash amount", MsgBoxStyle.Information, "Information ......")
                txtCash1.Focus()
                Exit Sub
            End If

            If Trim(cboPay_tearms.Text) = "CASH/FULL PAY" Then
                Value = CDbl(txtCash1.Text) + CDbl(txtDiscount.Text)
                If Value >= CDbl(txtInv_Amount.Text) Then
                Else
                    MsgBox("Please select the payment type", MsgBoxStyle.Information, "Information ......")
                    Exit Sub
                End If
            End If

        ElseIf Trim(cboPay_tearms.Text) = "CHEQUE" Then
            If Trim(cboBank1.Text) <> "" Then
            Else
                MsgBox("Please select the Bank Name", MsgBoxStyle.Information, "Information ......")
                cboBank1.ToggleDropdown()
                Exit Sub
            End If

            If Trim(txtChq1.Text) <> "" Then
            Else
                MsgBox("Please enter the cheque no", MsgBoxStyle.Information, "Information .......")
                txtChq1.Focus()
                Exit Sub
            End If

            If txtDOR.Text <> "" Then
            Else
                MsgBox("Please enter the Date of Realized", MsgBoxStyle.Information, "Information .......")
                txtDOR.Focus()
                Exit Sub


            End If

            If IsDate(txtDOR.Text) Then
            Else
                MsgBox("Please enter the correct realized date", MsgBoxStyle.Information, "Information .......")
                txtDOR.Focus()
                Exit Sub
            End If

            If txtAmount1.Text <> "" Then
            Else
                MsgBox("Please enter the chque amount", MsgBoxStyle.Information, "Information .......")
                txtAmount1.Focus()
                Exit Sub
            End If

            If IsNumeric(txtAmount1.Text) Then
            Else
                MsgBox("Please enter the correct cheque amount", MsgBoxStyle.Information, "Information .........")
                txtAmount1.Focus()
                Exit Sub
            End If
        End If
        Call Save_Data()
    End Sub

    Function Save_Data()
        Dim nvcFieldList1 As String

        Dim connection As SqlClient.SqlConnection
        Dim transaction As SqlClient.SqlTransaction
        Dim transactionCreated As Boolean
        Dim connectionCreated As Boolean
        SqlClient.SqlConnection.ClearAllPools()
        connection = DBEngin.GetConnection(True)
        connectionCreated = True
        transaction = connection.BeginTransaction()
        transactionCreated = True
        Dim MB51 As DataSet
        Dim _GetDate As DateTime
        Dim _Get_Time As DateTime
        Dim A As String
        ' Dim B As New ReportDocument
        Dim _Paytype As String
        Dim _Balance As Double

        Dim M01 As DataSet
        Dim M02 As DataSet
        Dim I As Integer
        Dim _remark As String
        Try


            If txtCash1.Text <> "" Then
            Else
                txtCash1.Text = "0"
            End If

            If txtAmount1.Text <> "" Then
            Else
                txtAmount1.Text = "0"
            End If

            If txtDiscount.Text <> "" Then
            Else
                txtDiscount.Text = "0"
            End If
            _GetDate = Month(txtDate1.Text) & "/" & Microsoft.VisualBasic.Day(txtDate1.Text) & "/" & Year(txtDate1.Text)

            _Get_Time = Month(Now) & "/" & Microsoft.VisualBasic.Day(Now) & "/" & Year(Now) & " " & Hour(Now) & ":" & Minute(Now)

            Call Load_EntryNo()

            nvcFieldList1 = "UPDATE P01Parameter SET P01No=P01No + " & 1 & " WHERE  P01Code='OU'"
            ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            nvcFieldList1 = "SELECT * FROM T16OutStanding WHERE T16Cus_Code='" & _cuscode & "' AND T16Ref_No='" & Trim(cboInvoice.Text) & "' "
            M01 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(M01) Then
                If Trim(cboPay_tearms.Text) = "CASH/FULL PAY" Then

                Else
                    _CASH_COM = 0
                    _CHQ_COM = 0
                End If

                nvcFieldList1 = "Insert Into T26Outstanding_Pay_Summery(T26Pay_No,T26Date,T26Net_Amount,T26Cash,T26Chq,T26Status)" & _
                                             " values('" & Trim(txtEntry.Text) & "','" & _GetDate & "', '" & txtPayment1.Text & "','" & txtCash1.Text & "','" & txtAmount1.Text & "','PAID')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into T25Outstanding_Collection(T25Pay_No,T25Ref_No,T25Inv_No,T25Date,T25Cus_Code,T25Pay_Type,T25Amount,T25Cash_Dis,T25Chq_Dis,T25Status)" & _
                                                     " values('" & Trim(txtEntry.Text) & "','" & Trim(cboInvoice.Text) & "', '" & Trim(M01.Tables(0).Rows(0)("T16Invoice_No")) & "','" & _GetDate & "','" & _cuscode & "','" & Trim(cboPay_tearms.Text) & "','" & (txtInv_Amount.Text) & "','" & _CASH_COM & "','" & _CHQ_COM & "','PAID')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                If Trim(cboPay_tearms.Text) = "CASH/FULL PAY" Or Trim(cboPay_tearms.Text) = "CASH PAY" Then
                    nvcFieldList1 = "Insert Into T16OutStanding(T16Date,T16Ref_No,T16Invoice_No,T16Tr_Type,T16Cus_Code,T16Remark,T16CR,T16DR,T16Status,T16Inv_Discount,T16Cash_Dis,T16Chq_Dis,T16Pay_no)" & _
                                                          " values('" & _GetDate & "','" & Trim(txtEntry.Text) & "', '" & Trim(M01.Tables(0).Rows(0)("T16Invoice_No")) & "','OUTSTANDING COLLECTION','" & _cuscode & "','-','0','" & CDbl(txtCash1.Text) & "','A','0','0','0','-')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                ElseIf Trim(cboPay_tearms.Text) = "CHEQUE" Then
                    nvcFieldList1 = "Insert Into T16OutStanding(T16Date,T16Ref_No,T16Invoice_No,T16Tr_Type,T16Cus_Code,T16Remark,T16CR,T16DR,T16Status,T16Inv_Discount,T16Cash_Dis,T16Chq_Dis,T16Pay_no)" & _
                                                       " values('" & _GetDate & "','" & Trim(txtEntry.Text) & "', '" & Trim(M01.Tables(0).Rows(0)("T16Invoice_No")) & "','OUTSTANDING COLLECTION','" & _cuscode & "','-','0','" & CDbl(txtAmount1.Text) & "','A','0','0','0','-')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)
                End If
            End If

            If Trim(cboPay_tearms.Text) = "CHEQUE" Then
                nvcFieldList1 = "Insert Into T17Chq_Transaction(T17Date,17Tr_No,T17Tr_Type,T17Invoice_No,T17Cus_Code,T17Chq_No,T17Bank_Name,T17DOR,T17Amount,T17Status)" & _
                                                   " values('" & _GetDate & "','" & txtEntry.Text & "', 'OUTSTANDING COLLECTION','" & cboInvoice.Text & "','" & _cuscode & "','" & txtChq1.Text & "','" & cboBank1.Text & "','" & txtDOR.Text & "','" & txtAmount1.Text & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            End If
            nvcFieldList1 = "Insert Into tmpTransaction_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                 " values('OUTSTANDING COLLECTION','SAVE', '" & _Get_Time & "','" & strDisname & "','" & txtEntry.Text & "')"
            ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            _remark = Trim(cboInvoice.Text) & "-" & Trim(cboCustomer.Text)
            nvcFieldList1 = "Insert Into T27Collection_Summery(T27Ref_No,T27Date,T27Tr_Type,T27Remark,T27Dr,T27Cr,T27Discount,T27Cash,T27Chq,T27Status)" & _
                                                                 " values('" & txtEntry.Text & "','" & _GetDate & "', 'OUTSTANDING COLLECTION','" & _remark & "','" & txtInv_Amount.Text & "','0','" & txtDiscount.Text & "','" & txtCash1.Text & "','" & txtAmount1.Text & "','A')"
            ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            'SALES COMMISSION
            nvcFieldList1 = "SELECT * FROM M01Setup_Company WHERE M01Ref_No='CE'"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "Insert Into T15Sales_Commission(T15Date,T15Invoice_No,T15Tr_Type,T15Cus_Code,T15Net_Amount,T15Chq_no,T15Cash_Com,T15Chq_Com,T15Collection_Com,T15Status)" & _
                                            " values('" & _GetDate & "','" & Trim(txtEntry.Text) & "', 'OUTSTANDING COLLECTION','" & _RefNo & "','" & CDbl(txtPayment1.Text) & "','-','0','0','" & MB51.Tables(0).Rows(0)("M01Cash_Collection") & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
            End If

            MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.ClearAllPools()
            connection.Close()
            Me.txtCash1.Text = ""
            Me.txtAmount1.Text = ""
            Me.txtChq1.Text = ""
            Me.txtDOR.Text = ""
            Me.cboBank1.Text = ""
            Me.cboPay_tearms.Text = ""
            Me.cboInvoice.Text = ""
            Me.txtInv_Amount.Text = ""
            Me.txtDiscount.Text = ""
            Me.txtPayment1.Text = ""
            txtAmount1.Text = "00.00"
            txtCash1.Text = "00.00"
            frmOutstandin_Collection_cnt.Load_Grid()
            Call Load_EntryNo()
            Call Load_Invoice()
            cboInvoice.ToggleDropdown()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.ClearAllPools()
                connection.Close()
            End If
        End Try

    End Function

    Private Sub cboInvoice_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles cboInvoice.InitializeLayout

    End Sub
End Class