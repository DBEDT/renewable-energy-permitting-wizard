<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logout.aspx.cs" Inherits="HawaiiDBEDT.Web.Profile.Logout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logout</title>
    <link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
    <link rel="stylesheet" media="screen" href="/include/css/jquery-ui.css" type="text/css" />
    <style>
        .ui-dialog-titlebar {
            display: none;
        }
        .ui-dialog .ui-dialog-buttonpane {
            text-align: center;
        }
        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none;
        }
    </style>
    <script language="javascript" type="text/javascript" src="/include/js/javascript.js"></script>
    <script type="text/javascript" src="/include/js/jquery-ui.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#modal').dialog({
                autoOpen: true,
                width: 400,
                height: 200,
                maxHeight: 200,
                modal: true,
                open: function (event, ui) {
                    $('body').css('overflow', 'hidden');
                },
                close: function (event, ui) {
                    $('body').css('overflow', 'visible');
                },
                buttons: {
                    "OK": function () {
                        $(this).dialog("close");
                        window.location = 'logout.aspx';
                    }
                }
            });
        });
    </script>
</head>
<body>
    <div id="pageWrapper">
        <div id="contentWrapper">
            <!-- BEGIN GLOBAL -->
            <!-- #include virtual="~/include/global.aspx" -->
            <!-- END GLOBAL -->
            <!-- BEGIN HEADER -->
            <!-- #include virtual="~/include/header.aspx" -->
            <!-- END HEADER -->
            <div>
                <div class="contentWrapperHome">
                    <a name="mainContent" id="mainContent"></a>
                    <form id="form1" runat="server">
                        <div id="modal" style="display: none">
                            <p align="center">
                                <p>Your session has timed out. You need to login and go to the My Evaluations page to access your saved evaluation that is currently in progress.</p>
                            </p>
                        </div>
                    </form>
                </div>
            </div>
            <!-- BEGIN FOOTER -->
            <!-- #include virtual="~/include/footer.aspx" -->
            <!-- END FOOTER -->
        </div>

    </div>
</body>
</html>
