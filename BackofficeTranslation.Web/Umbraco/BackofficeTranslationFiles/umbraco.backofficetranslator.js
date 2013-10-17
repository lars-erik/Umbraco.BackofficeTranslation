/*global angular, jQuery*/
(function (angular, $) {
	"use strict";
	var module = angular.module("umbraco.backofficetranslator", [])
		.directive("ngBlur", function ($parse) {
			return function (scope, element, attrs) {
				var fn = $parse(attrs["ngBlur"]);
				element.bind("blur", function (event) {
					if (scope.$$phase) {
						return;
					}
					scope.$apply(function () {
						fn(scope, { $event: event });
					});
				});
			};
		})
		.value("serviceUrls", {}),
	    exceptionInterceptor = function ($q, $window, $rootScope) {
		    return function (promise) {
			    promise.then(
				    function () {
				    },
				    function (reason) {
				    	$rootScope.errorPanel.dialog("option", "title", "Whoops: " + reason.data.ExceptionType);
					    $rootScope.errorPanel.html(reason.data.ExceptionMessage);
					    $rootScope.errorPanel.dialog("open");
				    }
			    );
			    return promise;
		    };
	    },
	    flexTextboxes = function () {
		    $("#translationPanel textarea").off("keyup");
		    $("#translationPanel textarea").each(function (i, tb) {
			    var jtb = $(tb);
			    jtb.height(jtb.prop("scrollHeight"));
		    });
		    $("#translationPanel textarea").on("keyup", function () {
			    var jtb = $(this);
			    jtb.height(jtb.prop("scrollHeight"));
		    });
	    },
	    scrollToArea = function (evt, ui) {
		    if (ui.newHeader.length === 0) {
			    return;
		    }
		    flexTextboxes();
		    $(document).scrollTop(ui.newHeader.offset().top);
	    };

	module.config(function ($httpProvider) {
		$httpProvider.responseInterceptors.push(exceptionInterceptor);
	});

	module.controller("ApplicationController", ["$scope", "$rootScope", "$element", "serviceRoot", "serviceSuffix", "serviceUrls", function ($scope, $rootScope, $element, serviceRoot, serviceSuffix, serviceUrls) {
		$($element).tabs({ disabled: [1] });
		$rootScope.errorPanel = $("#errorPanel", $($element));
		$rootScope.errorPanel.dialog({
			autoOpen: false,
			minWidth: 600,
			minHeight: 300,
			modal: true
		});

		$.extend(serviceUrls, {
			getAllFiles: serviceRoot + "files" + serviceSuffix + "/GetAllFiles",
			getPotentialFiles: serviceRoot + "files" + serviceSuffix + "/GetPotentialFiles",
			createFile: serviceRoot + "files" + serviceSuffix + "/Create",
			getComparison: serviceRoot + "comparison" + serviceSuffix + "/GetComparison",
			putTranslation: serviceRoot + "translation" + serviceSuffix + "/PutTranslation",
			deleteObsolete: serviceRoot + "translation" + serviceSuffix + "/DeleteObsolete"
		});
		$scope.autoUpdate = true;

		$scope.$on("ListFileSelected", function (evt, source, translation) {
			$scope.source = source;
			$scope.translation = translation;
			$($element).tabs("option", "disabled", []);
			$($element).tabs("select", 1);
			$("#loader", $($element)).attr("src", $("#loader", $($element)).attr("src"));
			$scope.$broadcast("FileSelected", source, translation);
		});
	}]);

	module.controller("FilesController", ["$scope", "$http", "serviceUrls", function ($scope, $http, serviceUrls) {
		function loadFiles() {
			$http.get(serviceUrls.getAllFiles).success(function (data) {
				$scope.files = data;
				$scope.source = $.grep(data, function (f) { return f.Name === "en"; })[0];
			});

			$http.get(serviceUrls.getPotentialFiles).success(function (data) {
				$scope.potential = data;
			});
		}

		$scope.noFileSelected = function () {
			return !$scope.selectedNew;
		};

		$scope.createFile = function () {
			if ($scope.noFileSelected()) {
				return;
			}
			$http.post(serviceUrls.createFile + "?cultureName=" + $scope.selectedNew.Name).success(function () {
				loadFiles();
				$scope.$emit("ListFileSelected", $scope.source, $scope.selectedNew);
			});
		};

		$scope.selected = function (translation) {
			$scope.$emit("ListFileSelected", $scope.source, translation);
		};

		loadFiles();
	}]);

	module.controller("TranslationController", ["$scope", "$rootScope", "$http", "serviceUrls", function ($scope, $rootScope, $http, serviceUrls) {
		var hasAccordion = false;

		function updateUi() {
			$("#translationPanel ul").accordion({
				autoHeight: false,
				collapsible: true,
				active: false,
				change: scrollToArea
			});
			hasAccordion = true;

			$(".different .icon").attr("title", "Probably translated");
			$(".equal .icon").attr("title", "Not translated");
			$(".new .icon").attr("title", "New");
			$(".obsolete .icon").attr("title", "Obsolete");
		}

		function loadFile(source, translation) {
			$http.get(serviceUrls.getComparison + "?sourceName=" + source.Name + "&translationName=" + translation.Name).success(function (data) {
				$(data.Areas).each(function (i, area) {
					$(area.Items).each(function (j, item) {
						item.OrigValue = item.TargetValue;
					});
				});
				$scope.comparison = data;
				$scope.loading = false;

				setTimeout(updateUi, 5);
			});
		}

		function countDifferences(area) {
			var i, stateCounts = {
				equal: 0,
				different: 0,
				"new": 0,
				obsolete: 0
			};

			for (i = 0; i < area.Items.length; i += 1) {
				stateCounts[area.Items[i].State] += 1;
			}

			area.States.Equal = stateCounts.equal;
			area.States.Different = stateCounts.different;
			area.States.Obsolete = stateCounts.obsolete;
			area.States.New = stateCounts["new"];
		}

		function onFileSelected(evt, source, translation) {
			if (hasAccordion) {
				$("#translationPanel ul").accordion("destroy");
			}
			$scope.loading = true;
			setTimeout(function () { loadFile(source, translation); }, 1);
		}
		
		$scope.loading = true;

		$scope.update = function (area, item) {
			if ($scope.$parent.autoUpdate && item.TargetValue !== item.OrigValue) {
				$http.put(serviceUrls.putTranslation, {
					file: $scope.$parent.translation.Name,
					area: area.Key,
					item: item.Key,
					newValue: item.TargetValue
				});
				item.OrigValue = item.TargetValue;
				if (item.SourceValue !== item.TargetValue) {
					if (item.SourceValue === "") {
						item.State = "obsolete";
					} else if (item.TargetValue === "") {
						item.State = "new";
					} else {
						item.State = "different";
					}
				} else {
					item.State = "equal";
				}
				countDifferences(area);
			}
			return;
		};

		$scope.deleteObsolete = function () {
			var source = $scope.$parent.source,
			    translation = $scope.$parent.translation;
			$http["delete"](serviceUrls.deleteObsolete + "?sourceName=" + source.Name + "&translationName=" + translation.Name)
				.success(function () { onFileSelected(null, source, translation); });
		};

		$scope.$on("FileSelected", onFileSelected);
	}]);

}(angular, jQuery));


