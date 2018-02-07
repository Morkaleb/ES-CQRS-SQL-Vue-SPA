<template>
  <section class="card">
    <div class="card-content">
      <strong>Here are the Days that will be owed</strong><hr/>
      <div v-for="manager in this.consequences">
        <div v-if="manager.Role">{{manager.Name}}
        </div>
    </div>
    </div>
  </section>
</template>

<script>
    export default {
      name: 'schedule-consequences',
      props: ['consequences']
    }
</script>

<style scoped>
  .card {
    border-radius: 5px;
    border: 1px solid darkgray;
    height: 500px;
  }
  strong {
    text-align: right;
  }
</style>
