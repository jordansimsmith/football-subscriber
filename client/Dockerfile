FROM node:current-alpine AS build

WORKDIR /app

COPY package.json yarn.lock ./
RUN yarn install
COPY . .
RUN yarn build

FROM node:current-alpine AS run

WORKDIR /app
ENV NODE_ENV=production

COPY package.json yarn.lock ./
RUN yarn install --production
COPY --from=build /app/.next ./.next

ENTRYPOINT ["yarn", "start"]

