<%@ Register TagPrefix="DBEDT" TagName="Login" Src="~/include/controls/login.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Renewable Energy Facility Permits in the State of Hawaii</title>
    <link rel="stylesheet" media="screen" href="/include/css/stylesheet.css" type="text/css" />
    <link rel="stylesheet" media="print" href="/include/css/print.css" type="text/css" />
    <script language="javascript" type="text/javascript" src="/include/js/javascript.js"></script>
</head>
<body>
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div id="pageWrapper">
        <!-- BEGIN GLOBAL -->
        <!-- #include virtual="~/include/global.aspx" -->
        <!-- END GLOBAL -->
        <!-- BEGIN HEADER -->
        <!-- #include virtual="~/include/header.aspx" -->
        <!-- END HEADER -->
        <div id="contentWrapper">
            <div>
                <div class="contentWrapperHome">
                    <h1>
                        Renewable Energy Permitting Wizard</h1>
                    <p>
                        Welcome to the Renewable Energy Permitting Wizard. This tool was developed to help
                        those proposing renewable energy projects understand the county, state, and federal
                        permits that may be required for their individual project. This tool works for projects
                        ranging in size from residential solar installations to large utility-scale facilities.
                        Since every project and permit is different, the results of this tool only represent
                        an initial appraisal of the necessary permitting requirements. <strong>Additional permitting
                            requirements may apply. The Energy Office strongly recommends consultation with
                            an expert.</strong></p>
                    <p>
                        The tool works by presenting a series of questions about the proposed project. Based
                        on your responses, a list of permits potentially required is displayed with typical
                        time frames for each permit. You may save the output as a BMP or PNG file.</p>
                    <p>
                        We suggest you pull your notes together in advance and be prepared to answer as
                        many as 80 questions in one session. If you register and login before starting your
                        evaluation session, your answers will be saved automatically as you advance. If
                        you do not login, your answers will be retained for 20 minutes of inactivity and
                        will be lost when you close your browser.
                    </p>
                    <a id="login" name="login"></a>
                    <asp:UpdatePanel ID="panel1" runat="server">
                        <ContentTemplate>
                            <DBEDT:Login ID="loginControl" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <p class="padTop10">
                        Input and comments to improve the Wizard are always welcomed. Please provide your feedback via the <a href="http://forms.hawaiicleanenergyinitiative.org/view.php?id=23" title="Renewable Energy Permitting Wizard Feedback">feedback survey</a>.
                    </p>
                    <p class="small">
                        <em>The Renewable Energy Permitting Wizard</em> was designed to help people understand
                        the permitting process. It is not a legal analysis and should not be relied on exclusively.
                        DBEDT strongly recommends users consult with an expert familiar with the relevant
                        permits and approvals to expedite permitting. The State of Hawaii is not responsible
                        for delays or losses should the processing of a permit or approval differ from the
                        results of the Wizard. DBEDT recommends directly contacting the permitting agencies
                        relevant to a specific project.</p>
                </div>
            </div>
            <!-- BEGIN FOOTER -->
            <!-- #include virtual="~/include/footer.aspx" -->
            <!-- END FOOTER -->
        </div>
    </div>
    </form>
</body>
</html>
