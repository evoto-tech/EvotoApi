export default function formatDateString (dateString) {
  return moment(dateString).format('DD/MM/YY [at] HH:mm')
}
