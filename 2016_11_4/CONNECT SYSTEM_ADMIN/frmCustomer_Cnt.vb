Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Imports System.Drawing.Image
Imports System.IO
Public Class frmCustomer_Cnt
    Dim _PrintStatus As String
    Dim _From As String
    Dim _TO As String
    Dim _Sales_ref As String
    Dim _Root As String

    Private Sub UsingDateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UsingDateToolStripMenuItem.Click
        OPR0.Visible = True
        Call Load_Sales_Ref()
        Call Load_Entry()
        cboDistrict.ToggleDropdown()
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Function Clear_text()
        Me.txtAddress.Text = ""
        Me.txtCR.Text = ""
        Me.txtNIC.Text = ""
        Me.txtShop_Name.Text = ""
        Me.txtTP.Text = ""
        Me.txtEmail.Text = ""
        Me.txtCus_Name.Text = ""
        Me.cboDistrict.Text = ""
        Me.cboRoot.Text = ""
        Me.cboSales_Ref.Text = ""
        Me.txtMobile.Text = ""
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
        Me.txtPic1.Text = ""
        Me.txtPic2.Text = ""
    End Function

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        Call Clear_text()
        OPR1.Visible = False
        cboFrom.Text = ""
        cboTo.Text = ""
        OPR0.Visible = False
        Panel2.Visible = False
        Call Load_Grid()
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Call Clear_text()
        Call Load_Entry()
    End Sub

    Function Load_District()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M15Name as [##] from M15District order by M15Province"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboDistrict
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 285
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

    Function Load_Sales_Ref()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M04Ref_Name as [##] from M04Create_Sale_Ref where M04Status='A' order by M04ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboSales_Ref
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 285
                '  .Rows.Band.Columns(1).Width = 160


            End With

            With cboFrom
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 285
                '  .Rows.Band.Columns(1).Width = 160


            End With

            With cboTo
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 285
                '  .Rows.Band.Columns(1).Width = 160


            End With
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Search_Sales_Ref() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from M04Create_Sale_Ref where M04Status='A' and M04Ref_Name='" & Trim(cboSales_Ref.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Sales_Ref = True
                _Sales_ref = Trim(M01.Tables(0).Rows(0)("M04Rer_No"))
            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Search_FROM_Sales_Ref() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from M04Create_Sale_Ref where M04Status='A' and M04Ref_Name='" & Trim(cboFrom.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_FROM_Sales_Ref = True
                _From = Trim(M01.Tables(0).Rows(0)("M04Rer_No"))
            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Search_TO_Sales_Ref() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from M04Create_Sale_Ref where M04Status='A' and M04Ref_Name='" & Trim(cboTo.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_TO_Sales_Ref = True
                _TO = Trim(M01.Tables(0).Rows(0)("M04Rer_No"))
            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Search_Root() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from M02New_Root where M02Status='A' and M02Root_Name='" & Trim(cboRoot.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_Root = True
                _Root = Trim(M01.Tables(0).Rows(0)("M02Root_Code"))
            End If
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function


    Function Load_Root()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M02Root_Name as [##] from M02New_Root where M02Status='A' and M02Dis_Name='" & cboDistrict.Text & "' order by M02ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboRoot
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 285
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

    Private Sub frmCustomer_Cnt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_District()
        Call Load_Sales_Ref()
        Call Load_Entry()
        Call Load_Grid()
        txtCode.ReadOnly = True
        txtCode.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        txtCR.Appearance.TextHAlign = Infragistics.Win.HAlign.Center

    End Sub

   
    Function Load_Entry()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='CU' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtCode.Text = "CU/00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtCode.Text = "CU/0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtCode.Text = "CU/" & M01.Tables(0).Rows(0)("P01No")
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

    Private Sub cboDistrict_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDistrict.AfterCloseUp
        Call Load_Root()
    End Sub

    Private Sub cboDistrict_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboDistrict.KeyUp
        If e.KeyCode = 13 Then
            cboRoot.ToggleDropdown()
        End If
    End Sub

    Private Sub cboDistrict_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDistrict.TextChanged
        Call Load_Root()
    End Sub

    Private Sub cboRoot_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboRoot.KeyUp
        If e.KeyCode = 13 Then
            cboSales_Ref.ToggleDropdown()
        End If
    End Sub

    Private Sub cboSales_Ref_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboSales_Ref.KeyUp
        If e.KeyCode = 13 Then
            txtShop_Name.Focus()
        End If
    End Sub

    Private Sub txtShop_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtShop_Name.KeyUp
        If e.KeyCode = 13 Then
            If Trim(txtShop_Name.Text) <> "" Then
                txtCus_Name.Focus()
            End If
        End If
    End Sub

    Private Sub txtCus_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCus_Name.KeyUp
        If e.KeyCode = 13 Then
            If Trim(txtCus_Name.Text) <> "" Then
                txtAddress.Focus()
            End If
        End If
    End Sub

    Private Sub txtAddress_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAddress.KeyUp
        If e.KeyCode = Keys.F1 Then
            txtNIC.Focus()
        End If
    End Sub

    Private Sub txtNIC_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNIC.KeyUp
        If e.KeyCode = 13 Then
            txtTP.Focus()
        End If
    End Sub

    Private Sub txtTP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTP.KeyUp
        If e.KeyCode = 13 Then
            txtMobile.Focus()
        End If
    End Sub

    Private Sub txtMobile_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMobile.KeyUp
        If e.KeyCode = 13 Then
            txtCR.Focus()
        End If
    End Sub

    Private Sub txtCR_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCR.KeyUp
        Dim Value As Double
        If e.KeyCode = 13 Then
            If IsNumeric(txtCR.Text) Then
                Value = txtCR.Text
                txtCR.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtCR.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
            End If
            txtEmail.Focus()
        End If
    End Sub

    Private Sub txtEmail_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmail.KeyUp
        If e.KeyCode = 13 Then
            cmdAdd.Focus()
        End If
    End Sub

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

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If Trim(cboDistrict.Text) <> "" Then
        Else
            MsgBox("Please select the District", MsgBoxStyle.Information, "Information ........")
            cboDistrict.ToggleDropdown()
            Exit Sub
        End If

        If Search_Root() = True Then
        Else
            MsgBox("Please select the Root Name", MsgBoxStyle.Information, "Information ........")
            cboRoot.ToggleDropdown()
            Exit Sub
        End If

        If Search_Sales_Ref() = True Then
        Else
            MsgBox("Please select the Sales Ref", MsgBoxStyle.Information, "Information ........")
            cboSales_Ref.ToggleDropdown()
            Exit Sub
        End If

        If Trim(txtShop_Name.Text) <> "" Then
        Else
            MsgBox("Please enter the Shop Name", MsgBoxStyle.Information, "Information ........")
            txtShop_Name.Focus()
            Exit Sub
        End If

        If Trim(txtCus_Name.Text) <> "" Then
        Else
            MsgBox("Please enter the Customer Name", MsgBoxStyle.Information, "Information ........")
            txtCus_Name.Focus()
            Exit Sub
        End If

        If txtAddress.Text <> "" Then
        Else
            txtAddress.Text = "-"
        End If

        If txtNIC.Text <> "" Then
        Else
            txtNIC.Text = "-"
        End If

        If txtTP.Text <> "" Then
        Else
            txtTP.Text = "-"
        End If

        If txtMobile.Text <> "" Then
        Else
            txtMobile.Text = "-"
        End If

        If txtEmail.Text <> "" Then
        Else
            txtEmail.Text = "-"
        End If

        If txtPic1.Text <> "" Then
        Else
            MsgBox("Please select the Shop Image", MsgBoxStyle.Information, "Information .........")
            Exit Sub
        End If

        If txtPic2.Text <> "" Then
        Else
            MsgBox("Please select the Street Image", MsgBoxStyle.Information, "Information .........")
            Exit Sub
        End If

        If txtCR.Text <> "" Then
        Else
            txtCR.Text = "0"
        End If

        If IsNumeric(txtCR.Text) Then
        Else
            MsgBox("Please enter the correct Credit Limit", MsgBoxStyle.Information, "Information .......")
            txtCR.Focus()
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
        Try

            nvcFieldList1 = "SELECT * FROM M05New_Customer WHERE M05Cus_Code='" & Trim(txtCode.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "update M05New_Customer set M05Distr_Name='" & Trim(cboDistrict.Text) & "',M05Root_Code='" & _Root & "',M05Ref_Code='" & _Sales_ref & "',M05Shop_Name='" & Trim(txtShop_Name.Text) & "',M05Address='" & Trim(txtAddress.Text) & "',M05NIC='" & Trim(txtNIC.Text) & "',M05Cus_Name='" & Trim(txtCus_Name.Text) & "',M05Contact_No='" & Trim(txtTP.Text) & "',M05Mobile_No='" & Trim(txtMobile.Text) & "',M05E_Mail='" & Trim(txtEmail.Text) & "',M05Cr_Limit='" & CDbl(txtCR.Text) & "',M05Status='A' where M05Cus_Code='" & Trim(txtCode.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                               " values('NEW_CUSTOMER','EDIT', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
            Else
                nvcFieldList1 = "update P01Parameter set P01No=P01No+ " & 1 & " where P01Code='CU' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into M05New_Customer(M05Cus_Code,M05Distr_Name,M05Root_Code,M05Ref_Code,M05Shop_Name,M05Address,M05NIC,M05Cus_Name,M05Contact_No,M05Mobile_No,M05E_Mail,M05Cr_Limit,M05Status)" & _
                                                                  " values('" & Trim(txtCode.Text) & "','" & Trim(cboDistrict.Text) & "', '" & _Root & "','" & _Sales_ref & "','" & Trim(txtShop_Name.Text) & "','" & Trim(txtAddress.Text) & "','" & Trim(txtNIC.Text) & "','" & Trim(txtCus_Name.Text) & "','" & Trim(txtTP.Text) & "','" & Trim(txtMobile.Text) & "','" & Trim(txtEmail.Text) & "','" & CDbl(txtCR.Text) & "','A')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                                " values('NEW_CUSTOMER','SAVE', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

            End If
            MsgBox("Record Update successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.Close()
            Call Update_Image()
            'Dim ms As New MemoryStream
            ''  ms.Dispose()
            'PictureBox2.Image.Save(ms, PictureBox2.Image.RawFormat)
            'Dim command As New SqlCommand("UPDATE S01Stock SET S01Img=@Img WHERE  S01id=" & Trim(M01.Tables(0).Rows(0)("S01id")) & " and S01Status='A'  and S01Tr_Type='ISU'", connection)
            'command.Parameters.Add("@img", SqlDbType.Image).Value = ms.ToArray()
            'connection.Open()
            'If command.ExecuteNonQuery() = 1 Then

            '    '  MsgBox("Records update Successfully", MsgBoxStyle.Information, "Information .......")

            'Else

            '    MsgBox("Records update unsuccessfully", MsgBoxStyle.Information, "Information .......")

            'End If
            'connection.Close()
            'ms.Dispose()


            Me.txtShop_Name.Text = ""
            Me.txtCus_Name.Text = ""
            Me.txtAddress.Text = ""
            Me.txtMobile.Text = ""
            Me.txtTP.Text = ""
            Me.txtCR.Text = ""
            Me.txtEmail.Text = ""
            txtShop_Name.Focus()
            Call Load_Grid()
            Call Load_Entry()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
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
            Sql = "SELECT * FROM M05New_Customer WHERE M05Cus_Code='" & Trim(txtCode.Text) & "'"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)

            If isValidDataset(M01) Then
               
                Dim ms As New MemoryStream
                Dim ms1 As New MemoryStream
                '  ms.Dispose()
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                PictureBox2.Image.Save(ms1, PictureBox2.Image.RawFormat)
                Dim command As New SqlCommand("UPDATE M05New_Customer SET M05Shop_Img=@Img,M05Street_Img=@Img1 WHERE  M05Cus_Code='" & Trim(txtCode.Text) & "'", connection)
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
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Call Clear_text()
        Call Load_Entry()
        OPR0.Visible = False
    End Sub


    Function Load_Grid_Root_Change()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  M02Root_Name as [Root Name],M05Cus_Code as [Customer Code],M05Shop_Name as [Shop Name]  from M05New_Customer inner join M02New_Root on M05Root_Code=M02Root_Code inner join M04Create_Sale_Ref on M04Rer_No=M05Ref_Code where M05Status='A' and M04Ref_Name='" & cboFrom.Text & "' order by M05ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid2.DataSource = M01
            UltraGrid2.Rows.Band.Columns(0).Width = 160
            UltraGrid2.Rows.Band.Columns(1).Width = 110
            UltraGrid2.Rows.Band.Columns(2).Width = 260
           
            ' UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  M04Ref_Name as [Sales Ref],M05Cus_Code as [Customer Code],M05Distr_Name as [District],M02Root_Name  as [Root Name],M05Shop_Name as [Shop Name],M05Contact_No as [Contact No]  from M05New_Customer inner join M02New_Root on M05Root_Code=M02Root_Code inner join M04Create_Sale_Ref on M04Rer_No=M05Ref_Code where M05Status='A' order by M05ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 120
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 120
            UltraGrid1.Rows.Band.Columns(3).Width = 180
            UltraGrid1.Rows.Band.Columns(4).Width = 260
            UltraGrid1.Rows.Band.Columns(5).Width = 120
            ' UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_Find()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  M04Ref_Name as [Sales Ref],M05Cus_Code as [Customer Code],M05Distr_Name as [District],M02Root_Name  as [Root Name],M05Shop_Name as [Shop Name],M05Contact_No as [Contact No]  from M05New_Customer inner join M02New_Root on M05Root_Code=M02Root_Code inner join M04Create_Sale_Ref on M04Rer_No=M05Ref_Code where M05Status='A' and M05Shop_Name like '%" & Trim(txtFind.Text) & "%' order by M05ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 120
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 120
            UltraGrid1.Rows.Band.Columns(3).Width = 180
            UltraGrid1.Rows.Band.Columns(4).Width = 260
            UltraGrid1.Rows.Band.Columns(5).Width = 120
            ' UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function

    Function Load_Grid_deactive()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection()
        Dim M01 As DataSet
        Try
            Sql = "select  M04Ref_Name as [Sales Ref],M05Cus_Code as [Customer Code],M05Distr_Name as [District],M02Root_Name  as [Root Name],M05Shop_Name as [Shop Name],M05Contact_No as [Contact No]  from M05New_Customer inner join M02New_Root on M05Root_Code=M02Root_Code inner join M04Create_Sale_Ref on M04Rer_No=M05Ref_Code where M05Status='I' order by M05ID"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            UltraGrid1.DataSource = M01
            UltraGrid1.Rows.Band.Columns(0).Width = 120
            UltraGrid1.Rows.Band.Columns(1).Width = 80
            UltraGrid1.Rows.Band.Columns(2).Width = 120
            UltraGrid1.Rows.Band.Columns(3).Width = 180
            UltraGrid1.Rows.Band.Columns(4).Width = 260
            UltraGrid1.Rows.Band.Columns(5).Width = 120
            ' UltraGrid1.Rows.Band.Columns(0).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center

            con.close()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
    End Function


    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        On Error Resume Next
        Dim _Row As Integer

        _Row = UltraGrid1.ActiveRow.Index
        OPR0.Visible = True
        txtCode.Text = Trim(UltraGrid1.Rows(_Row).Cells(1).Text)
        Call Search_Records()
        cboDistrict.ToggleDropdown()
    End Sub

    Function Search_Records()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double

        Try
            Sql = "select  *  from M05New_Customer inner join M02New_Root on M05Root_Code=M02Root_Code inner join M04Create_Sale_Ref on M04Rer_No=M05Ref_Code where M05Cus_Code='" & Trim(txtCode.Text) & "' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                cboDistrict.Text = Trim(M01.Tables(0).Rows(0)("M02Dis_Name"))
                cboRoot.Text = Trim(M01.Tables(0).Rows(0)("M02Root_Name"))
                cboSales_Ref.Text = Trim(M01.Tables(0).Rows(0)("M04Ref_Name"))
                txtShop_Name.Text = Trim(M01.Tables(0).Rows(0)("M05Shop_Name"))
                txtCus_Name.Text = Trim(M01.Tables(0).Rows(0)("M05Cus_Name"))
                txtAddress.Text = Trim(M01.Tables(0).Rows(0)("M05Address"))
                txtNIC.Text = Trim(M01.Tables(0).Rows(0)("M05NIC"))
                txtTP.Text = Trim(M01.Tables(0).Rows(0)("M05Contact_No"))
                txtMobile.Text = Trim(M01.Tables(0).Rows(0)("M05Mobile_No"))
                txtEmail.Text = Trim(M01.Tables(0).Rows(0)("M05E_Mail"))
                Value = Trim(M01.Tables(0).Rows(0)("M05Cr_Limit"))
                txtCR.Text = (Value.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture))
                txtCR.Text = (String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0,0.00}", Value))
                txtPic1.Text = "-"
                txtPic2.Text = "-"
                Dim arrayImage() As Byte = CType(M01.Tables(0).Rows(0)("M05Shop_Img"), Byte())
                Dim ms As New MemoryStream(arrayImage)
                PictureBox1.Image = Image.FromStream(ms)
                Dim arrayImage1() As Byte = CType(M01.Tables(0).Rows(0)("M05Street_Img"), Byte())
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

    Private Sub UltraButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton2.Click
        Dim A As String

        A = MsgBox("Are you sure you want to deactive this customer", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Deactive Customer .......")
        If A = vbYes Then
            Call Deactive_Customer()
        End If
    End Sub

    Function Deactive_Customer()
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

            nvcFieldList1 = "SELECT * FROM M05New_Customer WHERE M05Cus_Code='" & Trim(txtCode.Text) & "' "
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "update M05New_Customer set M05Status='A' where M05Cus_Code='" & Trim(txtCode.Text) & "' "
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                nvcFieldList1 = "Insert Into tmpMaster_Log(tmpStatus,tmpProcess,tmpTime,tmpUser,tmpCode)" & _
                                                               " values('NEW_CUSTOMER','DELETE', '" & Now & "','" & strDisname & "','" & txtCode.Text & "')"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)


            End If
            MsgBox("Record deactivate successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            connection.Close()

            Me.txtShop_Name.Text = ""
            Me.txtCus_Name.Text = ""
            Me.txtAddress.Text = ""
            Me.txtMobile.Text = ""
            Me.txtTP.Text = ""
            Me.txtCR.Text = ""
            Me.cboDistrict.Text = ""
            Me.cboSales_Ref.Text = ""
            Me.cboRoot.Text = ""
            Me.txtEmail.Text = ""
            cboDistrict.ToggleDropdown()
            Call Load_Grid()
            Call Load_Entry()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                connection.Close()
            End If
        End Try
    End Function

    Private Sub UltraButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton3.Click
        OPR1.Visible = False
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        OPR1.Visible = True
        cboFrom.Text = ""
        cboTo.Text = ""
        Call Load_Grid_Root_Change()
        cboFrom.ToggleDropdown()
    End Sub

    Private Sub cboFrom_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFrom.AfterCloseUp
        Call Load_Grid_Root_Change()
    End Sub

    Private Sub cboFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFrom.TextChanged
        Call Load_Grid_Root_Change()
    End Sub


    Private Sub UltraButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton1.Click
        If Trim(cboFrom.Text) = Trim(cboTo.Text) Then

            MsgBox("Please change a To Sales Ref", MsgBoxStyle.Information, "Information ........")
            Exit Sub
        End If

        If Search_FROM_Sales_Ref() = True Then
        Else
            MsgBox("Please select the From Sales Ref", MsgBoxStyle.Information, "Information ...........")
            Exit Sub
        End If

        If Search_TO_Sales_Ref() = True Then
        Else
            MsgBox("Please select the To Sales Ref", MsgBoxStyle.Information, "Information ...........")
            Exit Sub
        End If
        Call Save_Root_Change()

    End Sub

    Function Save_Root_Change()
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
        Dim I As Integer
        Dim m01 As DataSet

        Try
            nvcFieldList1 = "SELECT * FROM tmpSales_Ref_Change WHERE tmpTo_ref='" & _TO & "' AND tmpStatus='A'"
            m01 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(m01) Then
                nvcFieldList1 = "UPDATE tmpSales_Ref_Change SET tmpStatus='OK' WHERE tmpTo_ref='" & _TO & "' AND tmpStatus='A'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
            Else
                nvcFieldList1 = "SELECT * FROM M05New_Customer  inner join M04Create_Sale_Ref on M05Ref_Code=M04Rer_No WHERE M04Ref_Name='" & Trim(cboTo.Text) & "' "
                MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
                I = 0
                For Each DTRow1 As DataRow In MB51.Tables(0).Rows

                    nvcFieldList1 = "Insert Into tmpSales_Ref_Change(tmpCus_Code,tmpFrom_Ref,tmpTo_ref,tmpStatus)" & _
                                                                " values('" & Trim(MB51.Tables(0).Rows(I)("M05Cus_Code")) & "','" & _From & "', '" & _TO & "','A')"
                    ExecuteNonQueryText(connection, transaction, nvcFieldList1)

                    I = I + 1
                Next
            End If
            nvcFieldList1 = "select * from M04Create_Sale_Ref where M04Ref_Name='" & Trim(cboTo.Text) & "'"
            MB51 = DBEngin.ExecuteDataset(connection, transaction, nvcFieldList1)
            If isValidDataset(MB51) Then
                nvcFieldList1 = "UPDATE M05New_Customer SET M05Ref_Code='" & _TO & "' WHERE M05Ref_Code='" & _From & "'"
                ExecuteNonQueryText(connection, transaction, nvcFieldList1)
            End If
            MsgBox("Sales ref Change successfully", MsgBoxStyle.Information, "Information ..........")
            transaction.Commit()
            Call Load_Grid()
            connection.Close()
            OPR1.Visible = False
            cboFrom.Text = ""
            cboTo.Text = ""
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                DBEngin.CloseConnection(connection)
                connection.ConnectionString = ""
            End If
        End Try
    End Function


    Private Sub DeactiveCustomersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeactiveCustomersToolStripMenuItem.Click
        Call Load_Grid_deactive()
    End Sub

    Private Sub ByCustomerNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ByCustomerNameToolStripMenuItem.Click
        Panel2.Visible = True
        txtFind.Focus()
    End Sub

    Private Sub UltraButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton4.Click
        Panel2.Visible = False
    End Sub

    Private Sub txtFind_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFind.ValueChanged
        Call Load_Grid_Find()
    End Sub
End Class