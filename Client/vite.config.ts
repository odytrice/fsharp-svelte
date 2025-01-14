import { defineConfig } from 'vite'
import { svelte } from '@sveltejs/vite-plugin-svelte'

// https://vite.dev/config/
export default defineConfig({
  plugins: [svelte()],
  server: {
    port: 3000,
    proxy: {
      // redirect requests that start with /api/ to the server on port 5000
      '/api/': {
        target: 'http://localhost:5000',
        secure: false
      }
    }
  }
})
