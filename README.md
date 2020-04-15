[![wercker status](https://app.wercker.com/status/c21b8f7e1b269bb19530f00ad20053ad/s/master "wercker status")](https://app.wercker.com/project/byKey/c21b8f7e1b269bb19530f00ad20053ad)

# Event Processor

在事件溯源模式里，事件处理服务通常不提供 RESTful 端点。它负责处理一个或多个传入的事件流，基于事件执行处理过程，并将事件记录到事件存储设施中。传入事件的处事通常会产生一些传出的事件。

在《ASP.NET Core 微服务实战》一书的事件溯源/CQRS章节中所涉及的团队管理应用中，这一事件处理器用于响应传入的 **MemberLocationRecorded** 事件。这些事件都被记录到事件存储设施中，而位置也被提交到 *事实* 服务（在 CQRS 模式中，用于暴露高效查询的服务），在这*之后*，实际的事件处理过程将会执行。

## 处理位置事件

每个 **MemberLocationRecorded** 事件都包含 GPS 坐标信息、时间戳，还有一些其他元信息，比如事件报送来源等。报送的坐标将用于与其他团队成员的*当前*坐标作比较。如果事件处理器认为事件与某个团队成员相*接近*，它就会向另一个事件流产生一个 **ProximityDetectedEvent** 事件。

应用之后就可以按自己的方式任意响应 **ProximityDetectedEvent** 事件。在真实世界的应用中，这可能会包含向移动设备发送推送通知，通过第三方消息分发服务向响应式应用发出 WebSocket 风格的通知等等。


docker run -itd --name redis-test -p 6379:6379 redis
docker exec -it redis-test /bin/bash

docker run -p 6379:6379 -v $PWD/data:/data --name redis4SXD -d redis redis-server --appendonly yes
docker run --name redis4SXD -p 6379:6379 -d redis redis-server --appendonly yes --requirepass "398023"