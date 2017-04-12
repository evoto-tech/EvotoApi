export default function cleanValidationJson (json) {
  if (json && Array.isArray(json)) {
    return json.map((item) => {
      if (item.validation) {
        const newItemValidation = Object.assign({}, item.validation)
        Object.keys(newItemValidation).forEach((field) => {
          const fieldValue = newItemValidation[field]
          newItemValidation[field] = fieldValue === null || fieldValue === 'null' || fieldValue === '' ? '' : fieldValue
        })
        item.validation = newItemValidation
      }
      return item
    })
  }
}
