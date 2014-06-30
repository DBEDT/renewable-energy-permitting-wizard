<%@ Register TagPrefix="DBEDT" TagName="LoginStatus" Src="~/include/controls/global_nav.ascx" %>
<script type="text/javascript">
	$(document).ready(function () {
		$('h2').before('<a name="skiptocontent"></a>');
		// Reset Font Size
		var originalFontSize = $('html').css('font-size');
		$(".resetFont").click(function () {
			$('html').css('font-size', originalFontSize);
		});
		// Increase Font Size
		$(".increaseFont").click(function () {
			var currentFontSize = $('html').css('font-size');
			var currentFontSizeNum = parseFloat(currentFontSize, 10);
			var newFontSize = currentFontSizeNum * 1.2;
			$('html').css('font-size', newFontSize);
			return false;
		});
		// Decrease Font Size
		$(".decreaseFont").click(function () {
			var currentFontSize = $('html').css('font-size');
			var currentFontSizeNum = parseFloat(currentFontSize, 10);
			var newFontSize = currentFontSizeNum * 0.8;
			$('html').css('font-size', newFontSize);
			return false;
		});
	});
</script>
<div id="topgreen"><a href="#skiptocontent" tabindex="1"><strong>Skip to Content</strong></a>   Text size:  <a href="#" class="increaseFont"><strong>Larger</strong></a> / <a href="#" class="decreaseFont" ><strong>Smaller</strong></a> / <a href="#" class="resetFont" ><strong>Reset</strong></a></div>
<div id="globalNav">
  <ul>
    <li><a href="/">Home</a></li>
    <DBEDT:LoginStatus runat="server" />
  </ul>
</div>