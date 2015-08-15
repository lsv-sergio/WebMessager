WebMessagerApp.controller("WebMessagerController",
    function ($scope) {

        var reconnect = true;

        $scope.messagesList = [];
        $scope.usersList = [];
        $scope.userName = UserName;
        $scope.userId = UserId;
        $scope.newMessage = "";
        $scope.selectedReciverIndex = 0;
        $scope.selectedReciverId = 0;
        $scope.currentMessages = [];

        webMessagerHub = $.connection.webMessagerHub;

        isMessageValid = function () {
            return $scope.userId !== 0 && $scope.selectedReciverId >= 0 && $scope.newMessage.length > 0;
        }

        $scope.connect = function () {
            $.connection.hub.start().done(function () {
                webMessagerHub.server.connect($scope.userId);
            });
        }

        $scope.disconnect = function () {
            reconnect = false;
            $.connection.hub.stop();
            window.location.replace('/');
        }

        $scope.sendMessage = function () {
            if (isMessageValid) {
                webMessagerHub.server.sendMessage($scope.userId, $scope.selectedReciverId, $scope.newMessage);
                $scope.newMessage = "";
            }
        }

        $scope.getClass = function (index) {
            if (index === $scope.selectedReciverIndex)
                return 'btn-primary';
            else
                return 'btn-default';
        }

        getUsersHistory = function (reciverId) {

            var historyIndex = -1;
            if (reciverId < 0)
                return historyIndex;
            for (i = 0; i < $scope.messagesList.length; i++) {
                if ($scope.messagesList[i].UserId === reciverId) {
                    historyIndex = i;
                    break;
                }
            }
            return historyIndex;
        }

        addHistoryMessage = function (senderId, message) {
            var historyIndex = getUsersHistory(senderId);
            if (historyIndex === -1) {
                userHistory = { UserId: senderId, Messages: [] };
                userHistory.Messages.push(message);
                $scope.messagesList.push(userHistory);
            }
            else
                if (historyIndex >= 0) {
                    $scope.messagesList[historyIndex].Messages.push(message);
                }
        }

        saveCurrentMessages = function () {

            var historyIndex = getUsersHistory($scope.selectedReciverId);
            if (historyIndex === -1)
                $scope.currentMessages = [];
            else {
                var userHistory = $scope.messagesList[historyIndex];
                if (userHistory.Messages !== undefined)
                    $scope.currentMessages = userHistory.Messages;
                else
                    $scope.currentMessages = [];
            }
        }

        function setDivClass(newReciverId, oldReciverId) {

            if (oldReciverId >= 0) {
                var oldElement = $("div [data-id=" + oldReciverId + "]").first();
                if (oldElement !== undefined) {
                    $(oldElement).removeClass('btn-primary');
                    $(oldElement).addClass('btn-default');
                }
            }
            if (newReciverId >= 0) {
                var newElement = $("div [data-id=" + newReciverId + "]").first();
                if (newElement !== undefined) {
                    $(newElement).removeClass('btn-danger');
                    $(newElement).removeClass('btn-default');
                    $(newElement).addClass('btn-primary');
                }
            }
        }

        $scope.selectReciver = function (index) {

            if (index === $scope.selectedReciverIndex)
                return;

            var newUser = $scope.usersList[index];
            var newUserId = 0;
            if (newUser !== undefined) {
                newUserId = newUser.Id;
            }

            var historyIndex = getUsersHistory($scope.selectedReciverId);
            if (historyIndex === -1) {
                userHistory = { UserId: $scope.selectedReciverId, Messages: $scope.currentMessages };
                $scope.messagesList.push(userHistory);
            }
            else
                if (historyIndex >= 0) {
                    $scope.messagesList[historyIndex].Messages = $scope.currentMessages;
                }

            setDivClass(newUserId, $scope.selectedReciverId);

            $scope.selectedReciverId = newUserId;

            if (index === $scope.selectedReciverIndex)
                $scope.selectedReciverIndex = 0;
            else
                $scope.selectedReciverIndex = index;

            saveCurrentMessages();
        }

        SetupSignalR();

        notifyIncommingMessage = function (SenderId) {
            var element = $("div [data-id=" + SenderId + "]").first();
            if(element !== undefined)
            $(element).addClass('btn-danger');
        }

        function SetupSignalR() {

            webMessagerHub.client.incommingMessage = function (SenderId, Message) {

                var newMessage = angular.fromJson(Message);
                if ($scope.selectedReciverId === SenderId)
                    $scope.currentMessages.push(newMessage);
                else {
                    addHistoryMessage(SenderId, newMessage);
                    notifyIncommingMessage(SenderId);
                }
                $scope.$apply();
            }

            webMessagerHub.client.onNewUserConnected = function (userId, UserName) {
                $scope.usersList.push({ "Id": userId, "Name": UserName });
                $scope.$apply();
            };

            webMessagerHub.client.onClientDisconected = function (userId, userName) {
                var userListJson = angular.toJson($scope.usersList).replace(',{"Id":' + userId + ',"Name":"' + userName + '"}', '');
                $scope.usersList = angular.fromJson(userListJson);
                if (userId === $scope.selectedReciverId) {
                    $scope.selectReciver(0);
                }
                $scope.$apply();
            };

            webMessagerHub.client.onConnected = function (userId, userName, usersJson, messagesList) {

                $scope.userId = userId;
                $scope.userName = userName;
                $scope.usersList = angular.fromJson(usersJson);
                $scope.messagesList = angular.fromJson(messagesList);
                saveCurrentMessages();
                $scope.$apply();
                setDivClass(0, -1);
            };

        }
    })