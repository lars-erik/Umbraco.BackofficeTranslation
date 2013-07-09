<%@  Control Language="C#" AutoEventWireup="true" %>
<%@ Register Assembly="ClientDependency.Core" Namespace="ClientDependency.Core.Controls" TagPrefix="umb" %>

<script type="text/C#" language="c#" runat="server">
	
	protected void Page_Load(object s, EventArgs e)
	{
		Page.ClientScript.RegisterClientScriptBlock(
			GetType(),
			"translatorApiRoot",
			@"angular.module(""umbraco.backofficetranslator"")
				.value(""serviceRoot"", ""/Umbraco/BackofficeTranslation/"")
				.value(""serviceSuffix"", ""Api"");",
			true
		);
	}

</script>

<umb:JsInclude runat="server" FilePath="~/Umbraco/BackofficeTranslationFiles/angular.min.js" />
<umb:JsInclude runat="server" FilePath="~/Umbraco/BackofficeTranslationFiles/umbraco.backofficetranslator.js" />
<umb:CssInclude runat="server" FilePath="~/Umbraco/BackofficeTranslationFiles/translator.css" />
<umb:CssInclude runat="server" FilePath="propertypane/style.css" PathNameAlias="UmbracoClient" />
<umb:CssInclude runat="server" FilePath="ui/ui-lightness/jquery-ui.custom.css" PathNameAlias="UmbracoClient" />

<div class="propertypane">
	<div class="propertyItem">
		<div class="dashboardWrapper">

			<h2>Backoffice translation</h2>
			<img class="dashboardIcon" alt="Umbraco" src="dashboard/images/logo32x32.png" />


<div id="translator" ng-app="umbraco.backofficetranslator" ng-controller="ApplicationController">
	<ul>
		<li><a href="#filesPanel">Files</a></li>
		<li><a href="#translationPanel">Translation</a></li>
		<li><a href="#helpPanel">Help</a></li>
	</ul>

	<div id="filesPanel" ng-controller="FilesController">
		<p>
			<label for="source">Master file</label>
			<select name="source" id="source" ng-model="source" ng-options="file as file.EnglishName for file in files">
			</select>
		</p>
		
		<div id="existingFiles">
			<h3>Work with existing files</h3>
			<table class="fileList">
				<thead>
					<tr>
						<th class="nameCell">Name</th>
						<th class="languageCell">Language</th>
					</tr>
				</thead>
				<tbody ng-repeat="file in files">
					<tr ng-click="selected(file)">
						<td class="nameCell">{{file.Name}}</td>
						<td class="languageCell">{{file.EnglishName}}</td>
					</tr>
				</tbody>
			</table>
		</div>
		
		<div id="newFiles">
			<h3>Add a new language</h3>
			<select name="newFileCulture" ng-model="selectedNew" ng-options="file as file.EnglishName for file in potential">
				<option value="">-- Select culture for new file --</option>
			</select>
			<input type="button" ng-click="createFile()" ng-disabled="noFileSelected()" value="Create new file" />
		</div>
	</div>

	<div id="translationPanel" ng-controller="TranslationController">
		<p ng-show="loading" id="loader">
			<img src="Images/throbber.gif" />
			Please wait. Loading...
		</p>
		
		<div ng-hide="loading" id="translations">
			<div class="toolbar">
				<input type="button" ng-click="deleteObsolete()" value="Delete all obsolete" />
			</div>
			<ul ng-hide="loading">
				<li ng-repeat="area in comparison.Areas">
					<h2>
						{{area.Key}}
						<span class="differences">
							<span ng-show="area.States.Different > 0">
								<span class="different"><span class="icon"></span>{{area.States.Different}}</span>
							</span>
							<span ng-show="area.States.New > 0">
								<span class="new"><span class="icon"></span>{{area.States.New}}</span>
							</span>
							<span ng-show="area.States.Equal > 0">
								<span class="equal"><span class="icon"></span>{{area.States.Equal}}</span>
							</span>
							<span ng-show="area.States.Obsolete > 0">
								<span class="obsolete"><span class="icon"></span>{{area.States.Obsolete}}</span>
							</span>
						</span>
					</h2>
					<div>
					<table>
						<thead>
							<tr>
								<th class="iconCell"></th>
								<th class="itemKey">Key</th>
								<th class="sourceValue">{{$parent.$parent.source.EnglishName}}</th>
								<th class="targetValue">{{$parent.$parent.translation.EnglishName}}</th>
							</tr>
						</thead>
						<tbody>
							<tr ng-repeat="item in area.Items" ng-class="item.State">
								<td class="iconCell"><span class="icon"></span></td>
								<td class="itemKey">{{item.Key}}</td>
								<td class="sourceValue">{{item.SourceValue}}</td>
								<td class="targetValue">
									<textarea ng-model="item.TargetValue" ng-blur="update(area, item)"></textarea>
								</td>
							</tr>
						</tbody>
					</table>
					</div>
				</li>
			</ul>
		</div>
	</div>
	
	<div id="helpPanel">
		<ol>
			<li>Select or create a file under the files tab</li>
			<li>When using a master file other than English, make sure the selected master is current.</li>
			<li>Use the "delete all obsolete" button to make your file current.</li>
			<li>Changes are saved as you work.</li>
		</ol>
	</div>
	
	<div id="errorPanel">
	</div>

</div>


		</div>
	</div>
</div>