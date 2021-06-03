Imports System.Windows.Forms
Imports System.Collections
Imports System.Configuration
Imports System.Diagnostics
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinExplorerBar
Imports System.Data.SqlClient
Imports Infragistics.Win.UltraWinToolbars
Imports DBLotVbnet.common
Imports System.Net.NetworkInformation
Imports System.IO
Imports Infragistics.Win.UltraWinGrid
Imports Microsoft.VisualBasic.FileIO
Imports System.Net
Imports DBLotVbnet.DBEngin
Imports DBLotVbnet.DAL_InterLocation
Imports DBLotVbnet.DAL_Distributors
Imports CrystalDecisions.CrystalReports.Engine
'Imports Microsoft.Office.Interop.Excel
Imports System.Drawing
'Imports Spire.XlS
Imports DBLotVbnet.pln_Module
'Imports CrystalDecisions.CrystalReports.Engine
Public Class MDIMain 
    Dim networkcard() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()
    Dim dsUser As DataSet
    Dim A As String
    Dim B As New ReportDocument
    Dim exc As New Microsoft.Office.Interop.Excel.Application
    Dim Clicked As String
    'Dim workbooks As Workbooks = exc.Workbooks
    'Dim workbook As _Workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet)
    'Dim sheets As Sheets = workbook.Worksheets
    'Dim worksheet1 As _Worksheet = CType(sheets.Item(1), _Worksheet)
    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Global.System.Windows.Forms.Application.Exit()
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticleToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub
    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub
    Private m_ChildFormNumber As Integer = 0
#Region "UltraExplorerBar1_ItemClick"
    Private Sub UltraExplorerBar1_ItemClick(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinExplorerBar.ItemEventArgs) Handles UltraExplorerBar1.ItemClick
      

        Dim i As Integer
        Dim X As Integer
        Dim T01 As DataSet
        Dim DateDiffr As TimeSpan
        Dim _Fromdate As Date
        Dim _Todate As Date

        '  Dim B As New ReportDocument
        '============= This is if u want to close active form ========================
        ' ''For Each ChildForm As Form In Me.MdiChildren
        ' ''    ChildForm.Close()
        ' ''Next
        '==============================================================================
        VserverTime = Getservertime()

        'CType(Me.UltraToolbarsManager1.Tools("DataBase2"), LabelTool).SharedProps.Caption = "Server Date  : " & FormatDateTime(VserverTime, DateFormat.LongDate) & " : " & FormatDateTime(VserverTime, DateFormat.LongTime)

        '=========================================
        Select Case e.Item.Key

            Case "COM"
                frmCompany_Cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmCompany_Cnt.Show()
            Case "NROT"
                frmRoot_Cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmRoot_Cnt.Show()
            Case "NEWL"
                frmLocation_Cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmLocation_Cnt.Show()
            Case "SRF"
                frmSales_Ref_Cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmSales_Ref_Cnt.Show()

            Case "CUS"
                frmCustomer_Cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF".
                frmCustomer_Cnt.Show()
            Case "CUSDIS"
                frmCus_Discount.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmCus_Discount.Show()

            Case "SUP"
                frmSupplier_Cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmSupplier_Cnt.Show()
                'GRECI()
            Case "CNE"
                frmElectrician_cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmElectrician_cnt.Show()
            Case "VM"
                frmVehicle_Cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmVehicle_Cnt.Show()
            Case "COST"
                frmGRN_Budget.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmGRN_Budget.Show()
            Case "PDRC"
                frmCatogery_cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmCatogery_cnt.Show()
            Case "NRC"
                frmRow_Category_cntvb.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmRow_Category_cntvb.Show()
            Case "ITM"
                frmItem_cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmItem_cnt.Show()
            Case "BOM"
                frmBom_Creation.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmBom_Creation.Show()
            Case "KS"
                frmStockBalance_Product.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmStockBalance_Product.Show()
            Case "MOB"
                frmStockBalance_Mobile.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmStockBalance_Mobile.Show()
                'RPTTR()
            Case "WEA"
                frmStockBalance_Rowmaterial.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmStockBalance_Rowmaterial.Show()

            Case "MCC"
                frmGRN_connect.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmGRN_connect.Show()
            Case "PITM"
                frmGRN_Confirmation.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmGRN_Confirmation.Show()
            Case "RMII"
                frmRow_Issue.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmRow_Issue.Show()
            Case "PROIN"
                frmProduct_IN.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmProduct_IN.Show()

            Case "LOAD"
                frmLoading_cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmLoading_cnt.Show()
           

            Case "UNLO"
                frmUnloading_cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmUnloading_cnt.Show()

            

            Case "RPTP"
                frmOutstanding_Collection.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmOutstanding_Collection.Show()
          
          
            Case "PURR"
                frmElec_Commission.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmElec_Commission.Show()

         
            Case "STR"
                frmDeposit.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmDeposit.Show()
           
           
            Case "RLSU"
                frmsales_Cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmsales_Cnt.Show()
          

            Case "SB"
                frmWastage_cnt.MdiParent = Me
                m_ChildFormNumber += 1
                ' frmWinner.Text = "WNF"
                frmWastage_cnt.Show()

          

          
            Case "MDR"
                'frmDowntimereport.MdiParent = Me
                'm_ChildFormNumber += 1
                '' frmWinner.Text = "WNF"
                'frmDowntimereport.Show()
            Case "DRW"
                'frmRSDown.MdiParent = Me
                'm_ChildFormNumber += 1
                '' frmWinner.Text = "WNF"
                'frmRSDown.Show()

            

         


            Case "SD"
                'frmSD.MdiParent = Me
                'm_ChildFormNumber += 1
                '' frmWinner.Text = "WNF"
                'frmSD.Show()
            
            Case "CPR"
                'frmPI_Confirmation.MdiParent = Me
                'm_ChildFormNumber += 1
                '' frmWinner.Text = "WNF"
                'frmPI_Confirmation.Show()

            Case "DBP"
                'frmInvoiceLJ.MdiParent = Me
                'm_ChildFormNumber += 1
                '' frmWinner.Text = "WNF"
                'frmInvoiceLJ.Show()      
           
           
            

            
    
        End Select

    End Sub




#End Region
#Region "MDIMain_Load"

    Private Sub MDIMain_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
        SPL.Close()
        'frmViewPO.Close()
        'frmViewGRN.Close()
        Me.Close()
    End Sub
    'Function Load_Img()
    '    Dim _Date As Date
    '    Dim i As Integer
    '    Dim _Month As Integer
    '    Dim strFilePath As String

    '    _Date = Today
    '    For i = 1 To 6
    '        _Month = Month(_Date)

    '        'If Microsoft.VisualBasic.Day(_Date) <= 10 Then
    '        '    _Date = _Date.AddDays(+30)
    '        'ElseIf Microsoft.VisualBasic.Day(_Date) >= 20 Then
    '        '    _Date = _Date.AddDays(+15)
    '        'End If
    '        If i = 1 Then
    '            strFilePath = ConfigurationManager.AppSettings("imgPath") + "\Icons8-Windows-8-Animals-Gorilla.ico"

    '            UltraToolbarsManager1.Ribbon.Tabs(0).Groups(4).Tools(0).SharedProps.Caption = MonthName(_Month)
    '            UltraToolbarsManager1.Ribbon.Tabs(0).Groups(4).Tools(0).SharedProps.AppearancesSmall.Appearance.Image = strFilePath
    '            UltraToolbarsManager1.Ribbon.Tabs(0).Groups(4).Tools(0).Reset()
    '            '  UltraToolbarsManager1.Ribbon.Tabs(0).Groups(4).Tools(0).SharedProps.Dispose()
    '        End If
    '        If Microsoft.VisualBasic.Day(_Date) <= 10 Then
    '            _Date = _Date.AddDays(+30)
    '        ElseIf Microsoft.VisualBasic.Day(_Date) >= 20 Then
    '            _Date = _Date.AddDays(+15)
    '        End If
    '    Next
    'End Function

    Private Sub MDIMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SQL As String
        Dim con = New SqlConnection()
        SqlConnection.ClearAllPools()
        Try

            con = DBEngin.GetConnection()
            'Infragistics.Win.AppStyling.StyleManager.Load(ConfigurationManager.AppSettings("APPSTL"))
            Infragistics.Win.AppStyling.StyleManager.Load(ConfigurationManager.AppSettings("APPSTL"))
            '   Nav_Load()
            ' VserverTime = Getservertime()
            'TodayDraw()
            'Try
            'CType(Me.UltraToolbarsManager1.Tools("MName"), LabelTool).SharedProps.Caption = "Machine Name : " & (VMName) & " (" & netCard & ")"
            'CType(Me.UltraToolbarsManager1.Tools("MIP"), LabelTool).SharedProps.Caption = "Machine IP : " & VIP
            'CType(Me.UltraToolbarsManager1.Tools("MUser"), LabelTool).SharedProps.Caption = "Logged User : " & LoggedUser
            'CType(Me.UltraToolbarsManager1.Tools("DataSource2"), LabelTool).SharedProps.Caption = "Data Source : " & VDataSource & " : " & VDataBase
            'CType(Me.UltraToolbarsManager1.Tools("DataBase2"), LabelTool).SharedProps.Caption = "Server Date  : " & FormatDateTime(VserverTime, DateFormat.LongDate) & " : " & FormatDateTime(VserverTime, DateFormat.LongTime)

            Me.UltraToolbarsManager1.Ribbon.Tabs(0).Groups(2).Visible = False


            ' Catch
            ' MsgBox("Unable to Return your Network Details", MsgBoxStyle.Critical)
            'End Try
            '======================= Load C01Workstation Data to Main Menu
            ' Workstation_Load()
            '===========================================================
            'UltraExplorerBar1.Groups(0).Visible = False
            UltraExplorerBar1.Groups(1).Visible = False
            UltraExplorerBar1.Groups(2).Visible = False
            UltraExplorerBar1.Groups(3).Visible = False
            'UltraExplorerBar1.Groups(4).Visible = False
            ' UltraExplorerBar1.Groups(5).Visible = False
            'UltraExplorerBar1.Groups(6).Visible = False
            'UltraExplorerBar1.Groups(7).Visible = False
            con.ClearAllPools()
            con.close()

            SPL.Hide()
            frmLog.MdiParent = Me
            frmLog.Show()

            strKey = netCard
            '  Call frmCost.Load_Amount()
            ' Call Load_Img()
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
                con.ClearAllPools()
                con.close()

            End If


        End Try
    End Sub
#End Region

#Region "Ribbon Click"
    Private Sub UltraToolbarsManager1_ToolClick(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs)
        Select Case e.Tool.Key
            Case "cmb_mdi"
                'If Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "WNF" Then
                '    frmWinner.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmWinner.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "AGT" Then
                '    frmAgent_Maintenance.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmAgent_Maintenance.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "PPC" Then
                '    frmPayment_Cancel.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmPayment_Cancel.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "PDV" Then
                '    frmPayment_Duplicate.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmPayment_Duplicate.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "MCP" Then
                '    frmMiscellaneous.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmMiscellaneous.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "VOD" Then
                '    frmVoucher_Detailes.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmVoucher_Detailes.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "IST" Then
                '    frmInterLocation.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmInterLocation.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "ITD" Then
                '    frmVoucher_Detailes.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmVoucher_Detailes.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "IRD" Then
                '    frmVoucher_Detailes.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmVoucher_Detailes.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "ISP" Then
                '    frmVoucher_Detailes.MdiParent = Me
                '    m_ChildFormNumber += 1
                '    frmVoucher_Detailes.Show()
                'ElseIf Mid(CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).Text, 1, 3) = "XXX" Then
                '    For Each ChildForm As Form In Me.MdiChildren
                '        ChildForm.Close()
                '    Next
                '    Me.Close()
                'End If
        End Select
    End Sub
#End Region
    Sub Nav_Load()
        Dim i As Integer
        Dim con = New SqlConnection()
        Dim valueList As ValueList = New ValueList()

        '=======================================================================
        Try
            con = DBEngin.GetConnection()
            'dsUser = DBEngin.ExecuteDataset(con, Nothing, "Lst_Navigation", New SqlParameter("@cQryType", "NAV"))
            'For i = 0 To dsUser.Tables(0).Rows.Count - 1
            '    valueList.ValueListItems.Add(i, (dsUser.Tables(0).Rows(i)("SHT_Key") & " - " & (dsUser.Tables(0).Rows(i)("SHT_Description"))))
            'Next i
            'CType(Me.UltraToolbarsManager1.Tools("cmb_mdi"), ComboBoxTool).ValueList = valueList

            '===================================================================
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
        '=========================================================================
        ' "asdasd"
    End Sub
#Region "Workstation_Load"
    Sub Workstation_Load()
        Dim i As Integer
        ' Dim con = New SqlConnection()

        '=======================================================================
        Try
            '    con = DBEngin.GetConnection(True)
            'dsUser = DBEngin.ExecuteDataset(con, Nothing, "Lst_Navigation", New SqlParameter("@cQryType", "MAC"), New SqlParameter("@cMacCode", netCard))

            'For i = 0 To dsUser.Tables(0).Rows.Count - 1

            '    If dsUser.Tables(0).Rows(i)("c01systemstart") = "C" Then

            ' UltraExplorerBar1.Groups(0).Items(3).Visible = False
            '  UltraExplorerBar1.Groups(1).Items(4).Visible = False

            'UltraExplorerBar1.Groups(1).Items(4).Visible = False

            If Trim(strUGroup) = "PLN" Then
                UltraExplorerBar1.Groups(0).Visible = True
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(2).Items(0).Visible = False
                UltraExplorerBar1.Groups(2).Items(1).Visible = True
                UltraExplorerBar1.Groups(2).Items(4).Visible = True

           
                '   UltraExplorerBar1.Groups(4).Items(1).Visible = False
            ElseIf Trim(strUGroup) = "ADMIN" Then
                UltraExplorerBar1.Groups(0).Visible = True
                UltraExplorerBar1.Groups(1).Visible = True
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(4).Visible = True
                UltraExplorerBar1.Groups(3).Visible = True
                UltraExplorerBar1.Groups(5).Visible = True
                UltraExplorerBar1.Groups(2).Items(0).Visible = True
                UltraExplorerBar1.Groups(2).Items(1).Visible = True
                UltraExplorerBar1.Groups(2).Items(2).Visible = True
                UltraExplorerBar1.Groups(2).Items(3).Visible = True
                UltraExplorerBar1.Groups(2).Items(6).Visible = True
                'UltraExplorerBar1.Groups(2).Items(4).Visible = True
            ElseIf Trim(strUGroup) = "SUPPER" Then
                UltraExplorerBar1.Groups(0).Visible = True
                UltraExplorerBar1.Groups(1).Visible = True
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(4).Visible = True
                UltraExplorerBar1.Groups(3).Visible = True
                UltraExplorerBar1.Groups(2).Items(0).Visible = True
                UltraExplorerBar1.Groups(2).Items(1).Visible = False
                UltraExplorerBar1.Groups(2).Items(2).Visible = True
                UltraExplorerBar1.Groups(2).Items(3).Visible = False
                UltraExplorerBar1.Groups(3).Items(2).Visible = False

            ElseIf Trim(strUGroup) = "STORS" Then
                UltraExplorerBar1.Groups(0).Visible = True
                UltraExplorerBar1.Groups(1).Visible = True
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(4).Visible = True
                UltraExplorerBar1.Groups(3).Visible = True
                UltraExplorerBar1.Groups(2).Items(0).Visible = True
                UltraExplorerBar1.Groups(2).Items(1).Visible = False
                UltraExplorerBar1.Groups(2).Items(2).Visible = True
                UltraExplorerBar1.Groups(2).Items(3).Visible = True
                UltraExplorerBar1.Groups(3).Items(2).Visible = False
                UltraExplorerBar1.Groups(2).Items(6).Visible = False

            ElseIf Trim(strUGroup) = "PROCU" Then
                UltraExplorerBar1.Groups(0).Visible = False
                UltraExplorerBar1.Groups(1).Visible = False
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(3).Visible = False
                UltraExplorerBar1.Groups(4).Visible = False
                UltraExplorerBar1.Groups(2).Items(0).Visible = False
                UltraExplorerBar1.Groups(2).Items(1).Visible = False
                UltraExplorerBar1.Groups(2).Items(2).Visible = False
                UltraExplorerBar1.Groups(2).Items(3).Visible = False
                UltraExplorerBar1.Groups(2).Items(4).Visible = False
                UltraExplorerBar1.Groups(2).Items(5).Visible = False
                UltraExplorerBar1.Groups(2).Items(6).Visible = False
                UltraExplorerBar1.Groups(2).Items(7).Visible = False
                UltraExplorerBar1.Groups(2).Items(9).Visible = True
                UltraExplorerBar1.Groups(2).Items(10).Visible = False
                UltraExplorerBar1.Groups(2).Items(11).Visible = False
                UltraExplorerBar1.Groups(2).Items(8).Visible = False

                ' UltraExplorerBar1.Groups(0).Items(1).Visible = False

                UltraExplorerBar1.Groups(4).Visible = False
            ElseIf Trim(strUserLevel) = "09" Then
                UltraExplorerBar1.Groups(4).Visible = True
                UltraExplorerBar1.Groups(1).Visible = False
                UltraExplorerBar1.Groups(2).Visible = False
                UltraExplorerBar1.Groups(0).Visible = False
                UltraExplorerBar1.Groups(4).Items(0).Visible = False
                UltraExplorerBar1.Groups(4).Items(1).Visible = True
                UltraExplorerBar1.Groups(4).Items(2).Visible = True
                UltraExplorerBar1.Groups(4).Items(3).Visible = True
                UltraExplorerBar1.Groups(4).Items(5).Visible = False
                UltraExplorerBar1.Groups(4).Items(6).Visible = False
                UltraExplorerBar1.Groups(4).Items(7).Visible = False

                UltraExplorerBar1.Groups(3).Visible = False
            ElseIf Trim(strUserLevel) = "04" Then
                UltraExplorerBar1.Groups(0).Items(2).Visible = True
                UltraExplorerBar1.Groups(1).Items(4).Visible = True
                UltraExplorerBar1.Groups(0).Visible = True
                UltraExplorerBar1.Groups(1).Visible = True
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(4).Visible = True
                UltraExplorerBar1.Groups(3).Visible = True
                '  UltraExplorerBar1.Groups(3).Visible = True
            ElseIf Trim(strUserLevel) = "02" Then
                UltraExplorerBar1.Groups(0).Items(2).Visible = True
                UltraExplorerBar1.Groups(0).Visible = True
                UltraExplorerBar1.Groups(1).Visible = True
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(4).Visible = False

            ElseIf Trim(strUserLevel) = "03" Then
                '  UltraExplorerBar1.Groups(1).Items(4).Visible = False
                UltraExplorerBar1.Groups(1).Items(4).Visible = True
                UltraExplorerBar1.Groups(0).Visible = True
                UltraExplorerBar1.Groups(1).Visible = True
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(3).Visible = True
                UltraExplorerBar1.Groups(4).Visible = False
            ElseIf Trim(strUserLevel) = "05" Then
                UltraExplorerBar1.Groups(1).Items(0).Visible = False
                UltraExplorerBar1.Groups(1).Items(2).Visible = False
                UltraExplorerBar1.Groups(1).Items(1).Visible = False
                UltraExplorerBar1.Groups(1).Items(3).Visible = False
                UltraExplorerBar1.Groups(1).Items(4).Visible = False
                UltraExplorerBar1.Groups(1).Items(5).Visible = True
                UltraExplorerBar1.Groups(1).Items(6).Visible = False
                UltraExplorerBar1.Groups(1).Items(7).Visible = False
                UltraExplorerBar1.Groups(1).Items(8).Visible = False


                UltraExplorerBar1.Groups(0).Visible = False
                UltraExplorerBar1.Groups(1).Visible = True
                UltraExplorerBar1.Groups(2).Visible = False
                UltraExplorerBar1.Groups(3).Visible = True
                UltraExplorerBar1.Groups(4).Visible = True
                ' UltraExplorerBar1.Groups(3).Visible = True

                UltraExplorerBar1.Groups(3).Items(2).Visible = False
                UltraExplorerBar1.Groups(3).Items(3).Visible = False
                UltraExplorerBar1.Groups(3).Items(4).Visible = False
                UltraExplorerBar1.Groups(3).Items(6).Visible = False
                UltraExplorerBar1.Groups(3).Items(7).Visible = False
                UltraExplorerBar1.Groups(3).Items(8).Visible = False
                UltraExplorerBar1.Groups(3).Items(9).Visible = False
                ' UltraExplorerBar1.Groups(2).Items(10).Visible = False
                UltraExplorerBar1.Groups(3).Items(11).Visible = False
                UltraExplorerBar1.Groups(3).Items(12).Visible = False
                UltraExplorerBar1.Groups(3).Items(13).Visible = False
                UltraExplorerBar1.Groups(3).Items(14).Visible = False
                '  UltraExplorerBar1.Groups(2).Items(16).Visible = False
                UltraExplorerBar1.Groups(3).Items(18).Visible = True
                UltraExplorerBar1.Groups(3).Items(19).Visible = False
                UltraExplorerBar1.Groups(3).Items(20).Visible = False
                UltraExplorerBar1.Groups(3).Items(21).Visible = False
                UltraExplorerBar1.Groups(3).Items(22).Visible = False


            ElseIf Trim(strUserLevel) = "06" Then
                '  UltraExplorerBar1.Groups(0).Items(2).Visible = True
                UltraExplorerBar1.Groups(1).Items(4).Visible = True
                UltraExplorerBar1.Groups(1).Items(0).Visible = False
                UltraExplorerBar1.Groups(1).Items(5).Visible = False
                UltraExplorerBar1.Groups(1).Items(2).Visible = True
                UltraExplorerBar1.Groups(1).Items(3).Visible = True

                UltraExplorerBar1.Groups(0).Visible = False
                UltraExplorerBar1.Groups(1).Visible = True
                UltraExplorerBar1.Groups(2).Visible = True
                UltraExplorerBar1.Groups(3).Visible = True
                UltraExplorerBar1.Groups(4).Visible = False

            ElseIf Trim(strUserLevel) = "07" Then


                UltraExplorerBar1.Groups(0).Visible = False
                UltraExplorerBar1.Groups(1).Visible = False
                UltraExplorerBar1.Groups(2).Visible = False
                UltraExplorerBar1.Groups(3).Visible = False
                UltraExplorerBar1.Groups(4).Visible = True
                UltraExplorerBar1.Groups(4).Items(1).Visible = False
                UltraExplorerBar1.Groups(4).Items(1).Visible = False
                UltraExplorerBar1.Groups(4).Items(2).Visible = False
                UltraExplorerBar1.Groups(4).Items(3).Visible = False
                UltraExplorerBar1.Groups(4).Items(4).Visible = False
            Else
                'UltraExplorerBar1.Groups(0).Visible = False
                'UltraExplorerBar1.Groups(1).Visible = True
                'UltraExplorerBar1.Groups(2).Visible = False
                'UltraExplorerBar1.Groups(4).Visible = False

            End If
            'UltraExplorerBar1.Groups(1).Visible = True
            'UltraExplorerBar1.Groups(2).Visible = True
            'UltraExplorerBar1.Groups(3).Visible = True
            '  UltraExplorerBar1.Groups(4).Visible = True
            ' UltraExplorerBar1.Groups(5).Visible = True
            'UltraExplorerBar1.Groups(6).Visible = True
            'UltraExplorerBar1.Groups(7).Visible = True
            'UltraExplorerBar1.Groups(8).Visible = True
            'UltraExplorerBar1.Groups(9).Visible = True
            '    ElseIf dsUser.Tables(0).Rows(i)("c01systemstart") = "B" Then
            'UltraExplorerBar1.Groups(3).Visible = True
            'UltraExplorerBar1.Groups(4).Visible = False

            'UltraExplorerBar1.Groups(1).Enabled = False
            'UltraExplorerBar1.Groups(3).Enabled = True
            'UltraExplorerBar1.Groups(5).Enabled = False
            'UltraExplorerBar1.Groups(0).Enabled = False

            '    ElseIf dsUser.Tables(0).Rows(i)("c01systemstart") = "S" Then
            'UltraExplorerBar1.Groups(3).Visible = True
            'UltraExplorerBar1.Groups(4).Visible = True

            'UltraExplorerBar1.Groups(1).Enabled = False
            'UltraExplorerBar1.Groups(2).Enabled = False
            'UltraExplorerBar1.Groups(5).Enabled = False
            'UltraExplorerBar1.Groups(0).Enabled = False

            '    ElseIf dsUser.Tables(0).Rows(i)("c01systemstart") = "D" Then
            'UltraExplorerBar1.Groups(4).Visible = True
            'UltraExplorerBar1.Groups(5).Visible = True

            'UltraExplorerBar1.Groups(1).Enabled = False
            'UltraExplorerBar1.Groups(2).Enabled = False
            'UltraExplorerBar1.Groups(3).Enabled = False
            'UltraExplorerBar1.Groups(0).Enabled = False

            '    ElseIf dsUser.Tables(0).Rows(i)("c01systemstart") = "P" Then
            'UltraExplorerBar1.Groups(0).Visible = True
            'UltraExplorerBar1.Groups(4).Visible = True

            'UltraExplorerBar1.Groups(1).Enabled = False
            'UltraExplorerBar1.Groups(2).Enabled = False
            'UltraExplorerBar1.Groups(3).Enabled = False
            'UltraExplorerBar1.Groups(5).Enabled = False


            '    ElseIf dsUser.Tables(0).Rows(i)("c01systemstart") = "R" Then
            'UltraExplorerBar1.Groups(1).Visible = True
            'UltraExplorerBar1.Groups(4).Visible = True

            'UltraExplorerBar1.Groups(0).Enabled = False
            'UltraExplorerBar1.Groups(2).Enabled = False
            'UltraExplorerBar1.Groups(3).Enabled = False
            'UltraExplorerBar1.Groups(5).Enabled = False

            '===================================================================
        Catch returnMessage As Exception
            If returnMessage.Message <> Nothing Then
                MessageBox.Show(returnMessage.Message)
            End If
        End Try
        '=========================================================================

    End Sub
#End Region


#Region " UpdateStatusBar "

    Private Sub UpdateStatusBar(ByVal caption As String)

        ' strip ampersand
        Dim index As Integer = caption.IndexOf("&"c)
        If index <> -1 Then
            caption = caption.Remove(index, 1)

        End If

        Me.UltraStatusBar1.Panels("currentToolPanel").Text = caption

    End Sub 'UpdateStatusBar

#End Region


   
End Class
