<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      app
      clipped
    >
      <v-list dense>
        <v-list-item link>
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Dashboard</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item link>
          <v-list-item-action>
            <v-icon>mdi-settings</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Settings</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar
      app
      clipped-left
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title>Azure Functions Demo</v-toolbar-title>
    </v-app-bar>

    <v-content>
      <v-container
        class="fill-height"
        fluid
      >
    
      
  
        <v-row
          align="center"
          justify="center"
        >
          <v-col class="shrink">
            <input id="txtName" @keyup.enter="addMessage" v-model="txtInput" type="text">
                <v-btn                 
                  icon
                  large
                 v-on:click="getWinner"
                >
                Get Data 
                </v-btn>
          
          <di>{{pldata}}</di>
           
          </v-col>
          
        </v-row>
      </v-container>
    </v-content>

    <v-footer app>
      <span>&copy; 2019</span>
    </v-footer>
  </v-app>
</template>

<script>

 import axios from 'axios';

  export default {
    name:'app',
     data () {
      return{
      //drawer: null,
      pldata:" "
      }
      
    },

    
    methods: {
    getWinner: function () 
  {     
      axios.get("http://localhost:7071/api/OnDataReceived?name=2004")
      .then((response)  =>  {       
        this.pldata = response.data;
        console.log(this.pldata)
        
      })

      
    }   
  }
  }
</script>


