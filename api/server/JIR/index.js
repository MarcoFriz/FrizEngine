var
express   = require("express"),
Router    = express.Router(),
promise   = require('es6-promise').Promise,
sha1			= require('js-sha1'),
con				= require('../lib/connect.js').Connect('juanbosc_jir');
const bodyParser = require('body-parser');

//path      = require("path"),
indexFile = "jir.html"
//config



Router.use(bodyParser.json())
Router.use(bodyParser.urlencoded({ extended: true}))
//
Router.get("/",function(req,res){
  res.redirect(indexFile)
})

//new user
Router.get("/new/user",(req,res)=>{
	let digs = sha1.array(""+(new Date()).getTime());
	let id = "";
	//el tamaño maximo es de 20*3=60 digitos
	for (var i = 0; i < digs.length; i++) {
		id += digs[i];
	}
	//lo reducimos a 8 digitos
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
  console.log(`TODO - Alamacenar los datos del usuario:${name} (${id})`);
	let sql = "INSERT INTO `clients` (`clientId`, `clientName`) VALUES (?, ?)"
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

//connection
Router.get("/connection/:userId",(req,res)=>{
  var userId = req.params.userId;
  res.send(`{"actions": ["notifications","news","promotions", "checkData"]}`)
})
Router.post("/connection/:userId",(req,res)=>{
  var userId = req.params.userId;
  console.log("TODO - Establecer usarios conectados");//Puede que no sea necesario
  var success = true;
  var error = "null";
  res.send({"success": success, "error": error})
})

//notifications - Notificaciones para el usuario, ex: new friend o trofeo o mail
Router.get("/notifications/:userId",(req,res)=>{
  var userId = req.params.userId;
  console.log("TODO - Buscar las notifications del usuario ");//// TODO: Buscar las notifications
  res.send(`{'notifications': ['news/friends']}`)
})
Router.post("/notifications",(req,res)=>{
  var success = true;
  var error = "null";
  res.send({"success": success, "error": error})//no sirve para nada, podria usarlo para borrar la notifications
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
Router.get("/checkData",(req,res)=>{
  var userId = req.params.userId;
  console.log("TODO - Enviar informacion de chequeo al usuario");
  res.send(`{'check' : 'ok'}`)
})
Router.post("/checkData",(req,res)=>{
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)//no sirve para nada, podria usarlo para borrar la notifications
})
Router.post("/checkData/:userId",(req,res)=>{
  console.log("TODO - Anadir al amigo a la lista");
  var success = true;
  var error = "null";
  res.send(`{"success": ${success}, "error": ${error}}`)//no sirve para nada, podria usarlo para borrar la notifications
})
//friends
Router.get("/friends/:userId",(req,res)=>{
	var userId = req.params.userId;
	console.log("TODO - Enviar listado de nuevos amigos");
	res.send(`{'friends' : [12345,5678,90123]}`)//TODO enviar nombre de usuario, rescuer y score
})
module.exports = Router
