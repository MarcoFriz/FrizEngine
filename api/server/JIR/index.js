var
express   			= require("express"),
Router    			= express.Router(),
promise				= require('es6-promise').Promise,
sha1				= require('js-sha1'),
con					= require('../lib/connect.js').Connect('juanbosc_jir');
const bodyParser 	= require('body-parser');

//path = require("path"),
indexFile = "jir.html"
//	config

	Router.use(bodyParser.json())
	Router.use(bodyParser.urlencoded({ extended: true}))

	Router.get("/",function(req,res){
		res.redirect(indexFile)
	})

//	new user
	Router.get("/new/user",(req,res)=>{
		let digs = sha1.array(""+(new Date()).getTime());
		let id = "";
		// el tamaño maximo es de 20*3=60 digitos
		for (var i = 0; i < digs.length; i++) {
			id += digs[i];
		}
		// lo reducimos a 8 digitos
		var newId= "";
		for (var i = 0; i < 8; i++) {
			newId+= id[i];
		}
		var data = {"id": newId};
		res.send(data);
	})
	Router.post("/new/user",(req,res)=>{
		let id    = req.body.id;
		let name  = req.body.name;
		console.log(req.body);
		let sql = "INSERT INTO `clients` (`clientId`, `clientName`) VALUES (?, ?)";
		con.query(sql,[id,name],(err,result)=>{
			var error = err;
			var success = true;
			if(err){
				error = err;
				success = false;
			}
			var data = {"success": success, "error": error};
			res.send(data)
		})
	})
//	set user
	Router.get("/set/user",(req,res)=>{
		// esta opcion muestra que opciones son configurables para un usuario
		res.send({"options":["clientName","clientScore","clientImg","clientFbId","clientFbName"]});
	})
	Router.post("/set/user/:userId",(req,res)=>{
		// esta opcion muestra que opciones son configurables para un usuario
		var action = req.body.action; 
		console.log("setting user: "+action);
		var userId= req.params.userId;
		var clientName = req.body.clientName;
		var clientScore = req.body.clientScore;
		var clientImg = req.body.clientImg;
		var clientFbId= req.body.clientFbId;
		var clientFbName= req.body.clientFbName;
		var pairs = "";
		if(clientName!=undefined){
			pairs +=(pairs!="")?", ":"";
			pairs += "clientName = '"+clientName+"'";
		}
		if(clientScore!=undefined){
			pairs +=(pairs!="")?",":"";
			pairs += "clientScore = '"+clientScore+"'";
		}
		if(clientImg!=undefined){
			pairs +=(pairs!="")?", ":"";
			pairs += "clientImg = '"+clientImg+"'";
		}
		if(clientFbId!=undefined){
			pairs +=(pairs!="")?", ":"";
			pairs += "clientFbId = '"+clientFbId+"'";
		}
		if(clientFbName!=undefined){
			pairs +=(pairs!="")?", ":"";
			pairs += "clientFbName = '"+clientFbName+"'";
		}
		//Update normal
		if(action != "UpdateFbFriends"){
			let sql = "Update `clients` SET "+pairs+" WHERE clientId = ?";
			return con.query(sql,[userId],(err, result)=>{
				if(err){
					console.log(err);
					return res.send ({"success":false, "error":err});
				}
				res.send({"success":true,"error":null});
			})
		}
		//Update facebook Friends (si es que tiene)
		var friends = req.body.fbFriends;
		friends = JSON.parse(friends);
		if(req.body.fbFriends=="{}"){//no tiene amigos
			console.log("No tiene amigos en facebook");
			return res.send({"success":true,"error":null});
		}
		//si tuviera tenemos que analizar los datos
		//id, name
		res.send({"success":true,"error":null});
		
	})

//	connection
	Router.get("/connection/:userId",(req,res)=>{
		var userId = req.params.userId;
		res.send(`{"actions": ["notifications","news","promotions", "checkData"]}`)
	})
	Router.post("/connection/:userId",(req,res)=>{
		var userId = req.params.userId;
		console.log("TODO - Establecer usarios conectados");// Puede que no sea
		// necesario
		var success = true;
		var error = "null";
		res.send({"success": success, "error": error})
	})

//	notifications - Notificaciones para el usuario, ex: new friend o trofeo o
//	mail
	Router.get("/notifications/:userId",(req,res)=>{
		var userId = req.params.userId;
		console.log("TODO - Buscar las notifications del usuario ");// // TODO:
		// Buscar las
		// notifications
		res.send(`{'notifications': ['news/friends']}`)
	})
	Router.post("/notifications",(req,res)=>{
		var success = true;
		var error = "null";
		res.send({"success": success, "error": error})// no sirve para nada,
		// podria usarlo para borrar
		// la notifications
	})

//	news - Noticias
	Router.get("/news",(req,res)=>{
		var userId = req.body.id;
		console.log("TODO - Buscar las noticias");
		res.send("{'news': 'Hemos actualizado el servidor'}")
	})
	Router.post("/news",(req,res)=>{
		var success = true;
		var error = "null";
		res.send(`{"success": ${success}, "error": ${error}}`)// no sirve para
		// nada
	})

//	promotions - Noticias
	Router.get("/promotions",(req,res)=>{
		res.send("{'promotions': '¡¡¡Nuevos calcetines rescatados!!!'}")
	})
	Router.post("/promotions",(req,res)=>{
		var success = true;
		var error = "null";
		res.send(`{"success": ${success}, "error": ${error}}`)// no sirve para
		// nada
	})

//	CheckData
	Router.get("/checkData/:userId",(req,res)=>{
		let userId = req.params.userId;
		// revisamos que el usuario tenga su id registrada
		let sql = "SELECT * FROM `clients` WHERE clientId=?";
		con.query(sql, [userId], (err, result) => {
			let error = err;
			let success = true;
			let output = result;
			// Usuario no registrado o borrado
			if(result.length==0){
				console.log("usuario no registrado: "+ userId);
				// Se tiene que volver a registrar
				output="register";
				return res.send({"userId" : userId, "result" : output});
			}
			// Usuario sin nombre
			if(result[0].clientName == "" || result[0].clientName == null ||result[0].clientName == undefined  ){
				output="register";
				return res.send({"userId" : userId, "result" : output});
			}
			return res.send({"userId" : userId, "result" : output});
		});

	})
	Router.post("/checkData", (req,res)=>{
		var success = true;
		var error = "null";
		res.send(`{"success": ${success}, "error": ${error}}`)
		// no sirve para
		// nada, podria
		// usarlo para
		// borrar la
		// notifications
	})
	Router.post("/checkData/:userId",(req,res)=>{
		var success = true;
		var error = "null";
		res.send(`{"success": ${success}, "error": ${error}}`)
		// no sirve para
		// nada, podria
		// usarlo para
		// borrar la
		// notifications
	})

//	friends
	Router.get("/friends/:userId",(req,res)=>{
		var userId = req.params.userId;
		// buscamos a los amigos
		let sql = "SELECT * FROM `clients` WHERE clientId=?";
		con.query(sql,[userId],(err,result)=>{
			let success = true;
			if(err){
				success = false;
				return res.send({"success":success,"error":err});
			}
			// TODO procesar result
			console.log("TODO - Get Friends - Procesar resultado");
			let data;
			// case 1, no result
			// case 2, no friends
			// case 3, had friends
			// ----------------------------------\
			// case 1
			if(result.length==0){
				// el usuario no esta registrado
				// se debe pedir que se registre de nuevo
				data = {"success": false, "error": "register"};
				return res.send(data);
			}
			// ----------------------------------/
			// case 2
			// ----------------------------------\
			if(result[0].clientFriends==null){
				console.log("no tiene amigos");
				data ={
						"friends": [{
							"id":    result[0].clientId,
							"name":  result[0].clientName,
							"img":   result[0].clientImg,
							"score": result[0].clientScore,
							"fbId":  result[0].clientFbId,
							"fbName":result[0].clientFbName
						}],
						"success": true,
						"error": null
				};
				// Ahora agregamos a si mismo como amigo
				let friendList = [{"id":userId}];
				friendList = JSON.stringify(friendList);
				sql = "UPDATE  `clients` SET `clientFriends` = ? WHERE `clientId` = ?";
				return con.query(sql, [friendList, userId], (err2, result2)=>{
					if(err2){
						return res.send({"success": false, "error": err2, "extra": result2});
					}
					return res.send(data);
				});
			}
			// ----------------------------------/
			// case 3
			// ----------------------------------------------------------------------------\
			console.log("tiene amigos");
			/*
			 * friend format : { "name":"_name" ,"img":"_img " ,"fbId":"-1"
			 * ,"fbName":"_fbName" }
			 */
			let friends;

			friends= result[0].clientFriends;
			friends = JSON.parse(friends);
			// Por cada friend, se busca su informacion en la base de datos
			let str=friends[0].id;
			for (var i = 1; i < friends.length; i++) {
				str += "," + friends[i].id;
			}
			console.log(str);
			sql= "SELECT * FROM `clients` WHERE `clientId` IN ("+str+") ORDER BY `clientScore`";
			return con.query(sql, [str], (err2, result2)=>{
				if(err2){
					return res.send({"success":false, "error":err2});
				}
				let list=[];
				console.log(result2);
				for (var i = 0; i < result2.length; i++) {
					list.push({
						"id":     result2[i].clientId,
						"name":   result2[i].clientName,
						"img":    result2[i].clientImg,
						"score":  result2[i].clientScore,
						"fbId":   result2[i].clientFbId,
						"fbName": result2[i].clientFbName
					});
				}
				data = {"friends": list, "success": true, "error": null};
				console.log(data);
				return res.send(data);
			});
			// --------------------------------------------------------------------------/
		});
	});
Router.post("/friends/:userId", (req, res)=>{
	console.log("TODO - Post Friend ");
	var userId = req.params.userId;
	var action = req.body.action;
	var success = true;
	var error = "null";
	if(action=="UpdateFriend"){
		console.log("TODO - Update Friend - Fix Bug");
		var friendId = req.body.friendId;
		var friend = {"id" : friendId};
		var sql = "SELECT * FROM `clients` WHERE clientId=?";
		con.query(sql,[userId],(err,result)=>{
			console.log("Consultado base de datos");
			var error = err;
			var success = true;
			if(err){
				console.log("error");
				error = err;
				success = false;
				var data = {"friends": result, "success": success, "error": error};
				res.send(data);
				return;
			}
			// Si no tiene amigos lo agregamos
			console.log(result[0].clientFriends);
			if(result[0].clientFriends == null){
				console.log("Agregando Amigos");
				var friends = JSON.stringify([friend]);
				var sql2 = "UPDATE  `clients` SET `clientFriends` = ? WHERE `clientId` = ?";
				con.query(sql2, [friends, userId], (err2, result2)=>{
					var error = err2;
					var success = true;
					if(err2){
						success = false;
					}
					var data = {"success": success, "error": error};
					res.send(data);
					return;
				});
				return;
			}
			// Si tiene amigos
			var friends = result[0].clientFriends;
			friends = JSON.parse(friends);
			var hadFriend = false;
			for (var i = 0; i < friends.length; i++) {
				if(friends[i].id==friendId){
					hadFriend = true;
					break;
				}
			}
			// Si ya tiene al amigo se acabo
			if(hadFriend){
				console.log("El amigo ya esta en la lista");
				var data = {"success": success, "error": error};
				res.send(data);
				return;
			}
			// Si no lo agregamos
			friends.push({"id": friendId});
			// y lo enviamos
			var sql3 = "UPDATE  `clients` SET `clientFriends` = ? WHERE `clientId` = ?";
			friends = JSON.stringify(friends);
			con.query(sql3, [friends, userId], (err2, result2)=>{
				var error = err2;
				var success = true;
				if(err2){
					success = false;
				}
				var data = {"success": success, "error": error};
				res.send(data);
				return;
			});
		});
		return;
	}
	var data = {"success": success, "error": error};
	res.send(data);
})
module.exports = Router
