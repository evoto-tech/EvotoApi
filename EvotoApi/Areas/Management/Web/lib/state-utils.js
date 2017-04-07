export function insert (originalState, newItem) {
    let newState = originalState ? [].concat(originalState) : []
    newState.push(newItem)
    return newState
}

export function update (originalState, index, newValue) {
    let newState = [].concat(originalState)
    newState[index] = newValue
    return newState
}

export function remove (originalState, index) {
    let newState = [].concat(originalState)
    newState.splice(index, 1)
    return newState
}
