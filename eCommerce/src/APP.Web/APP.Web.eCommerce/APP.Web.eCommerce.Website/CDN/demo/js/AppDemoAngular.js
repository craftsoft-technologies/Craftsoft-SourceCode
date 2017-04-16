var app = angular.module('Demo', ['ngAnimate', 'ngAria', 'ngMaterial', 'ngMessages'])
    .run(function ($rootScope, $location, $http) {
        $rootScope.showLogin = false;
        $rootScope.showLoginPanel = function (param) {
            $rootScope.showLogin = param;
        }

        $rootScope.Logout = function (param) {
            window.location.href = '/';
        }
    });

app.controller('loginController', function ($scope, $location) {
    $scope.vm = {
        formData: {
            username: 'hello@patternry.com',
            password: 'foobar'
        },
        submit: function () {
            window.location.href = 'id/user';            
        }
    };

});

app.controller('dashboardController', function ($scope, $timeout, $mdSidenav, $log, $rootScope) {
    $scope.toggleLeft = buildDelayedToggler('left');
    $scope.toggleRight = buildToggler('right');
    $scope.isOpenRight = function(){
        return $mdSidenav('right').isOpen();
    };

    $rootScope.showLoginPanel = function (param) {
        //$rootScope.showLogin = param;
        //window.location.href = '/';
    }

    $('#btn-login').remove();
    $('#btn-register').remove();

    function debounce(func, wait, context) {
        var timer;

        return function debounced() {
            var context = $scope,
                args = Array.prototype.slice.call(arguments);
            $timeout.cancel(timer);
            timer = $timeout(function() {
                timer = undefined;
                func.apply(context, args);
            }, wait || 10);
        };
    }

    /**
     * Build handler to open/close a SideNav; when animation finishes
     * report completion in console
     */
    function buildDelayedToggler(navID) {
        return debounce(function() {
            // Component lookup should always be available since we are not using `ng-if`
            $mdSidenav(navID)
              .toggle()
              .then(function () {
                  $log.debug("toggle " + navID + " is done");
              });
        }, 200);
    }

    function buildToggler(navID) {
        return function() {
            // Component lookup should always be available since we are not using `ng-if`
            $mdSidenav(navID)
              .toggle()
              .then(function () {
                  $log.debug("toggle " + navID + " is done");
              });
        };
    }
})
  .controller('LeftCtrl', function ($scope, $timeout, $mdSidenav, $log) {
      $scope.close = function () {
          // Component lookup should always be available since we are not using `ng-if`
          $mdSidenav('left').close()
            .then(function () {
                $log.debug("close LEFT is done");
            });

      };
  })
  .controller('RightCtrl', function ($scope, $timeout, $mdSidenav, $log) {
      $scope.close = function () {
          // Component lookup should always be available since we are not using `ng-if`
          $mdSidenav('right').close()
            .then(function () {
                $log.debug("close RIGHT is done");
            });
      };
  });