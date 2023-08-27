'use strict';

/**
 * product controller
 */

const { createCoreController } = require('@strapi/strapi').factories;

module.exports = createCoreController('api::product.product');

/*
const { sanitize } = require('@strapi/utils');

module.exports = createCoreController('api::product.product', ({ strapi }) => ({
  async findOne(ctx) {
    const { id } = ctx.params;

    const entity = await strapi.entityService.findOne('api::product.product', 1, {
      fields: ['name', 'description', 'image'],
      populate: { category: true },
    });
    const sanitizedEntity = await sanitize.contentAPI.output(entity);

    return { data: sanitizedEntity };
  }
}));

module.exports = createCoreController('api::product.product', ({ strapi }) => ({
  async findMany(ctx) {

    const products = await strapi.entityService.findMany('api::product.product', {
      fields: ['name'],
      populate: { category: true },
    });

    console.log(products);

    return { data: products };
  },
  async findOne(ctx) {
    const { id } = ctx.params;

    const product = await strapi.entityService.findOne('api::product.product', id, {
      fields: ['name', 'description'],
      populate: { category: true },
    });
    console.log(product);
    return { data: product };
  }
}));
*/
