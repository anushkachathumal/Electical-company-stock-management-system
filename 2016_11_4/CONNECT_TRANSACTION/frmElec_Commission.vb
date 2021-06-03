Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinGrid
Imports DBLotVbnet.common
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports System.Configuration
Public Class frmElec_Commission
    Dim c_dataCustomer2 As DataTable

    Private Sub frmElec_Commission_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call read_only()
        Call Load_Gride2()
        Call Search()


        Me.txtDate1.Value = Date.Now
        txtEntry.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
        Call Load_Entry()
    End Sub

    Function Clear_Text()
        Me.cboTP.Text = ""
        Me.txtAddress.Text = ""
        Me.txtNIC.Text = ""
        Me.txtCard_No.Text = ""
        Me.txtE_Name.Text = ""
        Me.txtEntry.Text = ""

    End Function
    Function read_only()

        txtAddress.ReadOnly = True
        txtE_Name.ReadOnly = True
        txtNIC.ReadOnly = True

    End Function
    
    


    Function Load_Entry()
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select * from P01Parameter where  P01Code='EC' "
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                If M01.Tables(0).Rows(0)("P01No") >= 1 And M01.Tables(0).Rows(0)("P01No") < 10 Then
                    txtEntry.Text = "EC/00" & M01.Tables(0).Rows(0)("P01No")
                ElseIf M01.Tables(0).Rows(0)("P01No") >= 10 And M01.Tables(0).Rows(0)("P01No") < 100 Then
                    txtEntry.Text = "EC/0" & M01.Tables(0).Rows(0)("P01No")
                Else
                    txtEntry.Text = "EC/" & M01.Tables(0).Rows(0)("P01No")
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
    
    Function Search() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet

        Try
            Sql = "select M08Contact_No from M08Electritions"
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            With cboTP
                .DataSource = M01
                .Rows.Band.Columns(0).Width = 237
            End With

            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function
   

    Function Search_1() As Boolean
        Dim Sql As String
        Dim con = New SqlConnection()
        con = DBEngin.GetConnection(True)
        Dim M01 As DataSet
        Dim Value As Double
        Try
            Sql = "select M08Elc_Name from M08Electritions ='" & Trim(txtE_Name.Text) & ""
            M01 = DBEngin.ExecuteDataset(con, Nothing, Sql)
            If isValidDataset(M01) Then
                Search_1 = True


            End If

          
            con.close()

        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.close()
            End If
        End Try
    End Function

    Private Sub cboTP_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTP.AfterCloseUp
        Call Search_1()
    End Sub


    Private Sub cboTP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboTP.KeyUp
        If e.KeyCode = Keys.Enter Then
            txtCard_No.Focus()
        End If

    End Sub

    Private Sub txtDate1_BeforeDropDown(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDate1.BeforeDropDown
    End Sub

    Function Load_Gride2()
        Dim CustomerDataClass As New DAL_InterLocation()
        c_dataCustomer2 = CustomerDataClass.MakeDataTable_Elec_commission_cnt
        UltraGrid3.DataSource = c_dataCustomer2
        With UltraGrid3
            .DisplayLayout.Bands(0).Columns(0).Width = 90
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False
            .DisplayLayout.Bands(0).Columns(1).Width = 210
            .DisplayLayout.Bands(0).Columns(1).AutoEdit = False

            .DisplayLayout.Bands(0).Columns(2).Width = 80
            '  .DisplayLayout.Bands(0).Columns(3).Width = 110
            '.DisplayLayout.Bands(0).Columns(4).Width = 70
            '.DisplayLayout.Bands(0).Columns(5).Width = 70
            '.DisplayLayout.Bands(0).Columns(6).Width = 90


            .DisplayLayout.Bands(0).Columns(2).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            '  .DisplayLayout.Bands(0).Columns(3).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            '.DisplayLayout.Bands(0).Columns(4).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            '.DisplayLayout.Bands(0).Columns(5).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center
            ''  .DisplayLayout.Bands(0).Columns(7).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
            '.DisplayLayout.Bands(0).Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right


            .DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(1).CellActivation = Activation.NoEdit
            .DisplayLayout.Bands(0).Columns(2).CellActivation = Activation.NoEdit
            '  .DisplayLayout.Bands(0).Columns(3).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(4).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(5).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(6).CellActivation = Activation.NoEdit
            '.DisplayLayout.Bands(0).Columns(0).CellActivation = Activation.NoEdit

        End With
    End Function


    Private Sub txtEntry_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEntry.ValueChanged
        Call Load_Entry()


    End Sub


   


    Private Sub UltraButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton3.Click
        Call Clear_Text()

    End Sub

    Private Sub txtCard_No_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCard_No.ValueChanged
        txtCard_No.Appearance.TextHAlign = Infragistics.Win.HAlign.Center
    End Sub

    Private Sub txtE_Name_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtE_Name.ValueChanged

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Dim A As String
        A = MsgBox("Are you sure you want to exit this", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Exit .......")
        If A = vbYes Then
            Me.Close()
        End If
    End Sub

    Private Sub cboTP_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles cboTP.InitializeLayout
        Call Search()
    End Sub

    Private Sub UltraGrid3_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UltraGrid3.InitializeLayout

    End Sub
End Class