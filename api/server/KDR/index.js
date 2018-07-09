var
express   = require("express"),
Router    = express.Router(),
promise   = require('es6-promise').Promise,
sha1			= require('js-sha1');
//path      = require("path"),
indexFile = "kdr.html"
//config




//
Router.get("/",function(req,res){
  res.redirect(indexFile)
})

//new user
Router.get("/new/user",(req,res)=>{
	let hex = sha1.array(""+(new Date()).getTime());
	let id = "";
	//el tamaño maximo es de 20*3=60 digitos
	for (var i = 0; i < hex.length; i++) {
		id += hex[i];
	}
	var data = {"id": id};
  res.send(data);
})
Router.post("/new/user",(req,res)=>{
  var id    = req.body.id;
  var name  = req.body.name;
  console.log(`TODO - Alamacenar los datos del usuario:${name} (${id})`);
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)
})

//connection
Router.get("/connection/:userId",(req,res)=>{
  var userId = req.params.userId;
  res.send(`{"actions": ["notifications","news","promotions", "checkData/${userId}"]}`)
})
Router.post("/connection/:userId",(req,res)=>{
  var userId = req.params.userId;
  console.log("TODO - Establecer usarios conectados");//Puede que no sea necesario
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)
})

//notifications - Notificaciones para el usuario, ex: new friend o trofeo o mail
Router.get("/notifications/:userId",(req,res)=>{
  var userId = req.params.userId;
  console.log("TODO - Buscar las notifications del usuario ");
  res.send(`{'notifications': ['news/friends/${userId}']}`)
})
Router.post("/notifications",(req,res)=>{
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)//no sirve para nada, podria usarlo para borrar la notifications
})

//news - Noticias
Router.get("/news",(req,res)=>{
  var userId = req.body.id;
  console.log("TODO - Buscar las noticias");
  res.send("{'news': 'Hemos actualizado el servidor'}")
})
Router.post("/news",(req,res)=>{
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)//no sirve para nada
})

//promotions - Noticias
Router.get("/promotions",(req,res)=>{
  res.send("{'promotions': '¡¡¡Nuevos calcetines rescatados!!!'}")
})
Router.post("/promotions",(req,res)=>{
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)//no sirve para nada
})

//CheckData
Router.get("/checkData/:userId",(req,res)=>{
  var userId = req.params.userId;
  console.log("TODO - Enviar informacion de chequeo al usuario");
  res.send(`{'check' : 'ok'}`)
})
Router.post("/checkData/:userId",(req,res)=>{
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)//no sirve para nada, podria usarlo para borrar la notifications
})

//News friends
Router.get("/news/friends/:userId",(req,res)=>{
  var userId = req.params.userId;
  console.log("TODO - Enviar listado de nuevos amigos");
  res.send(`{'friends' : [12345,5678,90123]}`)//TODO enviar nombre de usuario, mascota y ranking
})
Router.post("/checkData/:userId",(req,res)=>{
  console.log("TODO - Anadir al amigo a la lista");
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)//no sirve para nada, podria usarlo para borrar la notifications
})
module.exports = Router
