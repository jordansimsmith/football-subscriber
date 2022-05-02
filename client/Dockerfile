FROM node:current-alpine AS build

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci
COPY . .
ARG NEXT_PUBLIC_SERVER_BASE
RUN npm run build

FROM node:current-alpine AS run

WORKDIR /app
ENV NODE_ENV=production

COPY package.json package-lock.json ./
RUN npm ci --production
COPY --from=build /app/.next ./.next
COPY public ./public
COPY next.config.js .

ENTRYPOINT ["npm", "run", "start"]

