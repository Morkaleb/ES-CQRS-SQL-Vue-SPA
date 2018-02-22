<template>
    <div class="dropdown">
        <button class="dropbtn">{{title}}</button>
        <div id="myDropdown" class="dropdown-content">
          <ul>
              <br />
              <li v-for="item in items" style="list-style:none; padding: 2px 0px; margin:0;"><a :href=item.link>{{item.title}}</a></li>
          </ul>
        </div>
    </div>
</template>

<script>
    export default {
        name:'dropdown',
        props: [
            'title', 'items'
        ],
        methods: {
        }
    }
</script>

<style>
    /* Dropdown Button */
    .dropbtn {
        background-color: #015351;
        color: white;
        padding: 16px;
        font-size: small;
        border: none;
        cursor: pointer;
        height:100%;
    }

        /* Dropdown button on hover & focus */
        .dropbtn:hover, .dropbtn:focus {
            background-color: #2980B9;
        }

    /* The container <div> - needed to position the dropdown content */
    .dropdown {
        position: relative;
        display: inline-block;
    }

    /* Dropdown Content (Hidden by Default) */
    .dropdown-content {
        display: none;
        position: absolute;
        background-color: #f1f1f1;
        min-width: 130px;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
        z-index: 1;
    }

        /* Links inside the dropdown */
        .dropdown-content a {
            color: black;
            text-decoration: none;
            display: block;
        }

            /* Change color of dropdown links on hover */
            .dropdown-content a:hover {
                background-color: #ddd
            }

    /* Show the dropdown menu (use JS to add this class to the .dropdown-content container when the user clicks on the dropdown button) */
    .show {
        display: block;
    } 
</style>
