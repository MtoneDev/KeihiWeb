﻿Imports Microsoft.AspNet.FriendlyUrls.Resolvers

Public Class ViewSwitcher
    Inherits System.Web.UI.UserControl

    Protected Property CurrentView() As String
        Get
            Return m_CurrentView
        End Get
        Private Set(value As String)
            m_CurrentView = value
        End Set
    End Property
    Private m_CurrentView As String

    Protected Property AlternateView() As String
        Get
            Return m_AlternateView
        End Get
        Private Set(value As String)
            m_AlternateView = value
        End Set
    End Property
    Private m_AlternateView As String

    Protected Property SwitchUrl() As String
        Get
            Return m_SwitchUrl
        End Get
        Private Set(value As String)
            m_SwitchUrl = value
        End Set
    End Property
    Private m_SwitchUrl As String

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        ' 現在のビューを判別します
        Dim isMobile = WebFormsFriendlyUrlResolver.IsMobileView(New HttpContextWrapper(Context))
        CurrentView = If(isMobile, "Mobile", "Desktop")

        ' 別のビューを判別します
        AlternateView = If(isMobile, "Desktop", "Mobile")

        ' ルートからスイッチ URL を作成します。たとえば ~/__FriendlyUrls_SwitchView/Mobile?ReturnUrl=/Page
        Dim url = GetRouteUrl("AspNet.FriendlyUrls.SwitchView", New With { _
            Key .view = AlternateView _
        })
        url += "?ReturnUrl=" & HttpUtility.UrlEncode(Request.RawUrl)
        SwitchUrl = url
    End Sub


End Class