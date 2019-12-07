lookup可以进行连接操作

[文档](https://docs.mongodb.com/manual/reference/operator/aggregation/lookup/index.html)

classes
```sh
db.classes.insert( [
   { _id: 1, title: "Reading is ...", enrollmentlist: [ "giraffe2", "pandabear", "artie" ], days: ["M", "W", "F"] },
   { _id: 2, title: "But Writing ...", enrollmentlist: [ "giraffe1", "artie" ], days: ["T", "F"] }
])
```
members
```
db.members.insert( [
   { _id: 1, name: "artie", joined: new Date("2016-05-01"), status: "A" },
   { _id: 2, name: "giraffe", joined: new Date("2017-05-01"), status: "D" },
   { _id: 3, name: "giraffe1", joined: new Date("2017-10-01"), status: "A" },
   { _id: 4, name: "panda", joined: new Date("2018-10-11"), status: "A" },
   { _id: 5, name: "pandabear", joined: new Date("2018-12-01"), status: "A" },
   { _id: 6, name: "giraffe2", joined: new Date("2018-12-01"), status: "D" }
])
```
关联
```
db.classes.aggregate([
   {
      $lookup:
         {
            from: "members",
            localField: "enrollmentlist",
            foreignField: "name",
            as: "enrollee_info"
        }
   }
])
```
输出
```sh
{
   "_id" : 1,
   "title" : "Reading is ...",
   "enrollmentlist" : [ "giraffe2", "pandabear", "artie" ],
   "days" : [ "M", "W", "F" ],
   "enrollee_info" : [
      { "_id" : 1, "name" : "artie", "joined" : ISODate("2016-05-01T00:00:00Z"), "status" : "A" },
      { "_id" : 5, "name" : "pandabear", "joined" : ISODate("2018-12-01T00:00:00Z"), "status" : "A" },
      { "_id" : 6, "name" : "giraffe2", "joined" : ISODate("2018-12-01T00:00:00Z"), "status" : "D" }
   ]
}
{
   "_id" : 2,
   "title" : "But Writing ...",
   "enrollmentlist" : [ "giraffe1", "artie" ],
   "days" : [ "T", "F" ],
   "enrollee_info" : [
      { "_id" : 1, "name" : "artie", "joined" : ISODate("2016-05-01T00:00:00Z"), "status" : "A" },
      { "_id" : 3, "name" : "giraffe1", "joined" : ISODate("2017-10-01T00:00:00Z"), "status" : "A" }
   ]
}
```