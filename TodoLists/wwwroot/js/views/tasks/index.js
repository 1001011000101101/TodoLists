
$(function () {
    var data = ejs.toJsObject(eval("(" + $("#scrData").html() + ")"));

    addTask = function (name, description, callback) {
        var r = $.rjson({
            url: host.arp + "Tasks/Add?Name=" + name + "&Description=" + description + "&ListID=" + data.list.id,
            data: {},
            success: function (result) {
                ejs.free(r);
                result = ejs.toJsObject(result);
                if (result.success) {
                    callback(result.id);
                    return;
                }
                ejs.alert("Ошибка", result.error || result.message);
            },
            error: function () {
                ejs.free(r);
                ejs.alert("Ошибка", "Произошла непредвиденная ошибка!");
            }
        });
        ejs.busy(r);
    };

    removeTask = function (id, callback) {
        var r = $.rjson({
            url: host.arp + "Tasks/Remove?ID=" + id,
            data: {},
            success: function (result) {
                ejs.free(r);
                result = ejs.toJsObject(result);
                if (result.success) {
                    callback();
                    return;
                }
                ejs.alert("Ошибка", result.error || result.message);
            },
            error: function () {
                ejs.free(r);
                ejs.alert("Ошибка", "Произошла непредвиденная ошибка!");
            }
        });
        ejs.busy(r);
    };

    editTask = function (id, name, description, completed, callback) {
        var r = $.rjson({
            url: host.arp + "Tasks/Edit?ID= " + id + "&Name=" + name + "&Description=" + description + "&Completed=" + completed,
            data: {},
            success: function (result) {
                ejs.free(r);
                result = ejs.toJsObject(result);
                if (result.success) {
                    callback();
                    return;
                }
                ejs.alert("Ошибка", result.error || result.message);
            },
            error: function () {
                ejs.free(r);
                ejs.alert("Ошибка", "Произошла непредвиденная ошибка!");
            }
        });
        ejs.busy(r);
    };

Vue.component('charttitle',{
  data() {
      return {
          message: '«' + data.list.name + '»',
      collapsed: true
    }
  },
  template:`
<div id ="chart_title">
    <label>
    <input v-model="message" placeholder="Insert the chart title" v-bind:class="[ {'is-collapsed' : collapsed }, 'inputHide' ]" type="text">
    </label>
    <h2>{{ message }} <button class="hideshow" v-on:click=" collapsed = !collapsed"><i class="fa fa-pencil" aria-hidden="true"></i></button></h2>
  </div>
`
})

/* Second header */

Vue.component('chartsubtitle',{
  data() {
    return { 
      message: 'Data reports',
      collapsed: true
    }
  },
  template:`
<div id ="chart_subtitle">
    <label>
    <input v-model="message" placeholder="Insert the chart title" v-bind:class="[ {'is-collapsed' : collapsed }, 'inputHide' ]" type="text">
    </label>
    <h2>{{ message }} <button class="hideshow" v-on:click=" collapsed = !collapsed"><i class="fa fa-pencil" aria-hidden="true"></i></button></h2>
  </div>
`
})


Vue.component('charttable',{
  props:['proplabels'],
  data() {
    return{
    disabled: true
    }
  },
  methods:{
      deleteEvent: function (index) {
          var obj = this;
          var id = obj.proplabels[index].id;

          var confirm = ejs.confirm("Внимание", " Вы уверены, что хотите удалить?", function () {
              removeTask(id, function () {
                  obj.proplabels.splice(index, 1);
              });
          });

          var buttons = confirm.dialog("option", "buttons");
          buttons[0].text = "Да";
          buttons[1].text = "Отмена";
          confirm.dialog("option", "buttons", buttons);
      },
      editEvent: function (index) {
          var obj = this;
          var id = obj.proplabels[index].id;
          var name = obj.proplabels[index].name;
          var description = obj.proplabels[index].description;
          var completed = obj.proplabels[index].completed;

          if (!obj.disabled) {
              editTask(id, name, description, completed, function () {
                  obj.disabled = !obj.disabled;
              });
          }
          else {
              obj.disabled = !obj.disabled;
          }
      }


},
  template:
    `
<div id="chart_table">
        <div class="top_titles">
            <div class="small-12 medium-1 column"> ID </div>
            <div class="small-12 medium-2 column"> Name </div>
            <div class="small-12 medium-6 column"> Description </div>
            <div class="small-12 medium-2 column"> Completed </div>
            <div class="small-12 medium-1 column">
                <i class="fa fa-trash" aria-hidden="true"></i>
            </div>
        </div>
    <div v-for="(val, index)al in proplabels" class="row table_cell">
        <div class="small-12 medium-1 column">
            <input type="text" v-model="val.id" disabled></input>
        </div>
        <div  class="small-12 medium-2 column single-cel">
            <input type="text" v-model="val.name" v-bind:disabled="disabled">
            </input>
        </div>
        <div class="small-12 medium-6 column single-cel">
            <input type="text" v-model="val.description" v-bind:disabled="disabled"> </input>
        </div>
        <div class="small-12 medium-2 column single-cel">
            <input type="checkbox" v-model="val.completed" v-bind:disabled="disabled"> </input>
        </div>
        <div class="small-12 medium-1 column edit_panel">
            <button @click="deleteEvent(index)">
                <i class="fa fa-times" aria-hidden="true"></i>
            </button>
<span class="edit_mode" @click="editEvent(index)">
                <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
            </span>
        </div>
    </div>


</div>

`
})


new Vue({
        el: '#chartpanel',
        data: {
            name: '',
            description: '',
            completed:'',
            labels: data.tasks,
          nextBarId: 1
        },
        computed: {
        },
        methods: {
            addRow: function (event) {
                var obj = this;

                addTask(obj.name, obj.description, function (id) {
                    var newRow = {
                        id: id,
                        name: obj.name,
                        description: obj.description,
                        completed: false
                    };

                    obj.labels.push(newRow);
                });

            }
        }
    });

});